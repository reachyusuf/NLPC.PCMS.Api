using System.Collections.Specialized;
using System.Dynamic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace NLPC.PCMS.Common.Utilities
{
    public static class Utility
    {
        private static readonly Random random = new Random();

        public static string getFirstNameFromArray(string fullName, int index)
        {
            string[] Names = fullName.Replace(" ", "  ").Trim().Split(separator: new[] { ' ' }, options: StringSplitOptions.RemoveEmptyEntries);
            return (Names.Length > index) ? Names[index] : fullName;
        }

        public static string GenerateReferenceCodes(int length = 14)
        {
            var guid = Guid.NewGuid().ToString().Replace("-", "").ToLower();
            return guid.Substring(0, length);
        }

        

        public static bool IsValidDate(string dt)
        {
            var flag = false;
            if (DateTime.TryParse(dt, out _))
            {
                flag = true;
            }
            else
            {

                flag = false;
            }
            return flag;
        }

        public static bool IsValidGuid(string guid)
        {
            return Guid.TryParse(guid, out var parsedGuid) && parsedGuid != Guid.Empty;
        }

        public static bool IsValidGuid(Guid guid)
        {
            return guid != Guid.Empty;
        }

        public static bool IsValidClientId(string clientId)
        {
            var allowedClients = new List<string> { "Web", "Mobile" };
            return allowedClients.Contains(clientId, StringComparer.OrdinalIgnoreCase);
        }

        

       

       

        public static bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return false;

            string pattern = @"^(?=.*[A-Z])(?=.*[a-zA-Z0-9])(?=.*[!@#$%^&*(),.?\"":{}|<>]).{6,}$";

            // Check if the password matches the pattern
            return !Regex.IsMatch(password, pattern);
        }

        public static ExpandoObject QueryStringToObject(string queryString)
        {
            NameValueCollection queryParams = HttpUtility.ParseQueryString(queryString);
            dynamic dynamicObject = new ExpandoObject();
            foreach (string key in queryParams.AllKeys)
            {
                ((IDictionary<string, object>)dynamicObject)[key] = queryParams[key];
            }

            return dynamicObject;
        }

        public static string UploadBase64File(string base64filestring, string fileName = null)
        {
            fileName = fileName ?? $"{CreateNewGUIDCodes()}.{GetFileExtensionFromBase64string(base64filestring)}";
            string filePath = GetFilePath(fileName);
            File.WriteAllBytes(filePath, Convert.FromBase64String(base64filestring));
            return filePath;
        }

        public static string GetFileExtensionFromBase64string(string base64String)
        {
            var data = base64String.Substring(0, 5);

            switch (data.ToUpper())
            {
                case "IVBOR":
                    return "png";
                case "/9J/4":
                    return "jpg";
                case "AAAAF":
                    return "mp4";
                case "JVBER":
                    return "pdf";
                case "AAABA":
                    return "ico";
                case "UMFYI":
                    return "rar";
                case "E1XYD":
                    return "rtf";
                case "U1PKC":
                    return "txt";
                case "MQOWM":
                case "77U/M":
                    return "srt";
                default:
                    return string.Empty;
            }
        }

        public static string GetFilePath(string filePath)
        {
            var dirPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            dirPath = Path.GetDirectoryName(dirPath + filePath);
            return dirPath;
        }

        public static string GetMimeType(string base64Str)
        {
            var regex = new Regex(@"data:(?<mime>[\w/\-\.]+);(?<encoding>\w+),(?<data>.*)", RegexOptions.Compiled);
            var match = regex.Match(base64Str);
            var mimex = match.Groups["mime"].Value;
            return mimex;
        }

        public static bool IsValidFileUploaded(string base64Str, string extension, string allowedExtension, int allowedSize, out string msg)
        {
            if (string.IsNullOrWhiteSpace(extension))
            {
                msg = "Uploaded file has no extension.";
                return false;
            }

            var allowedExtensions = allowedExtension.Split(',');
            if (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains<string>(extension))
            {
                msg = $"Uploaded File extension not allowed. Try to upload file in these format ({string.Join(", ", allowedExtensions)})";
                return false;
            }

            var imageBytes = Convert.FromBase64String(base64Str);
            var fileSize = (imageBytes.Length / (1024 * 1024));
            if (fileSize > allowedSize)
            {
                msg = $"Uploaded File size too large. File size cannot be larger than ({allowedSize} MB)";
                return false;
            }

            msg = string.Empty;
            return true;
        }

        public static int GetDateDifference(DateTime endDate, DateTime startDate)
        {
            return int.Parse(endDate.Subtract(startDate).TotalMinutes.ToString());
        }

        public static long UnixTimeStamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        public static string EncodeBase64(this string value)
        {
            var valueBytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(valueBytes);
        }

        public static string DecodeBase64(this string value)
        {
            var valueBytes = System.Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(valueBytes);
        }

        public static string UrlEncode(this string value)
        {
            var urlString = HttpUtility.UrlEncode(value);
            return urlString;
        }

        public static string UrlDecode(this string value)
        {
            var urlString = HttpUtility.UrlDecode(value);
            return urlString;
        }

        public static string ExtractNumbers(string expr)
        {
            return string.Join(null, System.Text.RegularExpressions.Regex.Split(expr, "[^\\d]"));
        }

        public static DateTime ReturnCurrentLocalDateTime()
        {
            return DateTime.UtcNow.AddHours(1);
        }

        public static string ReturnImageTimeStamp()
        {
            return $"?ts={DateTime.Now.ToString()}";
        }

        public static string CreateUniqueCodes(int intSize, string _default = null)
        {
            string UniqueNo = string.Empty;

            if (!string.IsNullOrEmpty(_default))
            {
                UniqueNo = _default;
            }

            UniqueNo += Guid.NewGuid().ToString().Replace("-", "").ToLower();
            UniqueNo = UniqueNo.Substring(0, intSize);

            return UniqueNo;
        }

        public static string CreateNewGUIDCodes()
        {
            return Guid.NewGuid().ToString().Replace("-", "").ToLower();
        }

        public static decimal CalculatePercentage(decimal amount, decimal percentage)
        {
            if (amount <= 0 || percentage <= 0)
                return 0;

            return (amount * percentage) / 100;
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                string DomainMapper(Match match)
                {
                    var idn = new IdnMapping();
                    string domainName = idn.GetAscii(match.Groups[2].Value);
                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException) { return false; }
            catch (ArgumentException) { return false; }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException) { return false; }
        }

        public static bool IsValidMobileNumber(string mobileNo)
        {
            return Regex.IsMatch(mobileNo, @"^([0-9]{11})$"); // @"^([1-9]\d{10})?$");
                                                              //&& Regex.IsMatch(email, @"^(?=.{1,64}@.{4,64}$)(?=.{6,100}$).*");
        }

        public static string GetFirstNameFromArray(string fullName, int index)
        {
            string[] Names = fullName.Trim().Replace("  ", " ").Split(separator: new[] { ' ' }, options: StringSplitOptions.RemoveEmptyEntries);
            return (Names.Length > index) ? Names[index] : fullName;
        }

        public static string GetInitials(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                return string.Empty;
            }

            var _initials = string.Empty;
            //var _index = 0;
            string[] Names = fullName.Trim().Replace(",", " ").Replace("  ", " ").Split(separator: new[] { ' ' }, options: StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in Names)
            {
                _initials += item.Substring(0, 1).ToUpper();
                //_index += 1;
            }
            return _initials;
        }

        public static string GenerateOTP()
        {
            return random.Next(100000, 999999).ToString();
        }
    }
}
