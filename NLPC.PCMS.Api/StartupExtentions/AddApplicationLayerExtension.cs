namespace NLPC.PCMS.Api.StartupExtentions
{
    public static class AddApplicationLayerConfig
    {
        public static IServiceCollection AddApplicationLayerExtension(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }
    }
}
