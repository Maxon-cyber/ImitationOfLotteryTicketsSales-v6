using YamlDotNet.Serialization;

namespace Deserialize.YamlDeserialize;

public sealed class DeserializerYaml<TModel> : IDeserialiser<TModel>
    where TModel : class
{
    public TModel DeserializeConfiguringFile(string filePath, INamingConvention namingConvention)
        => new DeserializerBuilder()
        .WithNamingConvention(namingConvention)
        .Build()
        .Deserialize<TModel>(File.ReadAllText(filePath));
}