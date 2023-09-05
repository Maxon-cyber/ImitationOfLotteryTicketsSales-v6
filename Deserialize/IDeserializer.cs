using YamlDotNet.Serialization;

namespace Deserialize;

public interface IDeserializer<TModel>
    where TModel : class
{
    public TModel DeserializeConfiguringFile(string path, INamingConvention namingConvetion);
}