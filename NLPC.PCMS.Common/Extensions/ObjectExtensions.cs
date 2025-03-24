using Newtonsoft.Json;

namespace NLPC.PCMS.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static T Deserialize<T>(this string jsonString)
        where T : new()
        {
            return string.IsNullOrWhiteSpace(jsonString) is false ? JsonConvert.DeserializeObject<T>(jsonString) : new T();
        }

        public static string Serialize(this object @object)
        {
            return JsonConvert.SerializeObject(@object);
        }

        public static Dictionary<string, object> ObjectToDictionary(object obj)
        {
            var dictionary = new Dictionary<string, object>();

            // Get all the properties of the object using reflection
            var properties = obj.GetType().GetProperties();

            // Loop through each property and add it to the dictionary
            foreach (var property in properties)
            {
                dictionary.Add(property.Name, property.GetValue(obj)!);
            }

            return dictionary;
        }
    }
}
