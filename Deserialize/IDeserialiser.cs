using YamlDotNet.Serialization;

namespace Deserialize;

public interface IDeserialiser<TModel>
    where TModel : class
{
    public TModel DeserializeConfiguringFile(string path, INamingConvention namingConvetion);
}