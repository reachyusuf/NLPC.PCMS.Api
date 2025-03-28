using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.Formatters;
using NLPC.PCMS.Api.FiltersAndMiddlewares;
using NLPC.PCMS.Api.StartupExtentions;
using NLPC.PCMS.Common.DTOs;
using NLPC.PCMS.Common.Utilities;
using Serilog;
using Serilog.Events;
using System.Reflection;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{envName}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

var appSettingsConfig = config.GetSection("AppSettings");
var appSettings = appSettingsConfig.Get<AppSettingsDto>() ?? throw new Exception("invalid appettings or couldn't bind config file");

// Configure logging based on the environment
var loggerConfiguration = new LoggerConfiguration()
    .ReadFrom.Configuration(config)
    .MinimumLevel.Is(config.GetValue("Logging:LogLevel:Default", LogEventLevel.Information))
    .Enrich.FromLogContext();

var connString = config.GetConnectionString("ConnectionString");

if (appSettings?.RateLimit?.Enabled is true)
{
    // Add services for rate limiting
    builder.Services.AddRateLimiter(options =>
    {
        options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
            RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: context.User?.Identity?.Name ?? context.Request.Headers.Host.ToString(),
                factory: _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = appSettings!.RateLimit.PermitLimit, // Allow 100 requests
                    Window = TimeSpan.FromMinutes(appSettings.RateLimit.Window), // In a 1-minute window
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 0 // appSettings.RateLimit.QueueLimit // Queue up to 2 requests
                }));

        options.RejectionStatusCode = StatusCodes.Status429TooManyRequests; // Too many requests response
    });
}


if (config.GetValue<bool>("Logging:Seq:Enabled") is true)
{
    loggerConfiguration.WriteTo.Seq(
        serverUrl: appSettings!.Seq.ServerUrl,
        apiKey: appSettings.Seq.ApiKey);
}

if (config.GetValue<bool>("Logging:File:Enabled") is true)
{
    loggerConfiguration.WriteTo.File(
        path: config["Logging:File:Path"]!,
        rollingInterval: Enum.Parse<RollingInterval>(config["Logging:File:RollingInterval"]!, true));
}

if (config.GetValue<bool>("Logging:Console:Enabled") is true)
{
    loggerConfiguration.WriteTo.Console();
}

Log.Logger = loggerConfiguration.CreateLogger();

builder.Host.UseSerilog();
//--

builder.Services.AddHttpContextAccessor();
builder.Services.AddApplicationLayerExtension();
builder.Services.AddDBContextExtension(config);
builder.Services.AddIdentityManagerExtension(appSettings!);
builder.Services.AddDIRegistrationExtension(config, envName!, appSettings!);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure FluentValidation to disable default data annotations validation
builder.Services.AddFluentValidationAutoValidation(opt => opt.DisableDataAnnotationsValidation = true);
builder.Services.AddValidatorsFromAssembly(Assembly.Load("Mware.CollegeDreams.Validator"));

// Add controllers and other necessary services
builder.Services.AddControllers(options =>
{
    options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
    options.Filters.Add(typeof(ValidateFilterAttribute));
});

builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

//--adds httpclient
builder.Services.AddHttpClient("Myhttp", client =>
{
    //client.BaseAddress = new Uri(appSettings.PenneyApiBaseUrl);
    client.Timeout = TimeSpan.FromSeconds(60);
});

// Register HttpClientHelperUtil
builder.Services.AddTransient<HttpClientHelperUtil>();
//-----------------


// Configure the HTTP request pipeline.
var app = builder.Build();

if (appSettings?.RateLimit?.Enabled is true)
{
    // Use rate limiter middleware
    app.UseRateLimiter();
}

var env = app.Environment;
if (env.IsDevelopment() is true)
{
    app.UseDeveloperExceptionPage();
}

//app.UseSerilogRequestLogging();
app.UseRouting();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

// Global middleware registration
app.UseMiddleware<ExceptionHandlingMiddleware>();


app.MapControllers();

app.Run();







//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();
