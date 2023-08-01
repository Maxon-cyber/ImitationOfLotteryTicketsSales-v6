using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Deserialize.YamlDeserialize;

public class DeserializerYaml<TModel> : IDeserialiser<TModel>
    where TModel : class
{
    public TModel DeserializeConfiguringFile(string path)
    {
        IDeserializer deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();

        return deserializer.Deserialize<TModel>(path);
    }
}