using SharpYaml.Serialization;

namespace AzureMagic.Storage.TableStorage.Helpers.Yaml
{
    public class YamlConverter
    {
        public static string Serialize(object value)
        {
            var settings = new SerializerSettings
            {
                PreferredIndent = 4,
                SortKeyForMapping = false
            };

            var serializer = new Serializer(settings);
            var yaml = serializer.Serialize(value);

            return yaml;
        }
    }
}