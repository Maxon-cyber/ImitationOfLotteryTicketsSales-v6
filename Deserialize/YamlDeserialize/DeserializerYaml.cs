using YamlDotNet.Serialization;

namespace Deserialize.YamlDeserialize;

internal sealed class DeserializerYaml<TModel> : IDeserializer<TModel>
    where TModel : class
{
    public TModel DeserializeConfiguringFile(string filePath, INamingConvention namingConvention)
        => new DeserializerBuilder()
        .WithNamingConvention(namingConvention)
        .Build()
        .Deserialize<TModel>(File.ReadAllText(filePath));
}