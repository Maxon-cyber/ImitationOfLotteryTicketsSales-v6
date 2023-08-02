using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Deserialize.YamlDeserialize;

public class DeserializerYaml<TModel> : IDeserialiser<TModel>
    where TModel : class
{
    public ResultDeserialize<TModel> DeserializeConfiguringFile(string path) =>
            new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build()
            .Deserialize<ResultDeserialize<TModel>>(path);
}