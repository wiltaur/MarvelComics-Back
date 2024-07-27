using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WAppMarvelComics.Domain.Custom
{
    public static class JsonSerializationExtensionMethods
    {

        /// <summary>
        /// Sets the necessary settings and safely deserialize an object.
        /// </summary>
        /// <param name="json"></param>
        /// <returns>
        /// </returns>
        public static T? SecureDeserializeObject<T>(this string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.None
                    });
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al deserializar Json. ", ex);
            }
           
        }

        /// <summary>
        /// Sets the necessary configurations and serializes an object.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ignoreNulls"></param>
        /// <returns>
        /// </returns>
        public static string SecureSerializeObject<T>(this T obj, bool? ignoreNulls = null)
        {
            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(), // Esto convierte las propiedades en minúsculas
                NullValueHandling = ignoreNulls.HasValue && ignoreNulls.Value ? NullValueHandling.Ignore : NullValueHandling.Include
            };
            return JsonConvert.SerializeObject(obj, Formatting.Indented, jsonSettings);
        }


        /// <summary>
        /// Serializes an object without settings.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>
        /// </returns>
        public static string SecureSimpleSerializeObject<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

    }
}
