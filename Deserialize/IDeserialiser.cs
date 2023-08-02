namespace Deserialize;

public interface IDeserialiser<TModel>
    where TModel : class
{
    public ResultDeserialize<TModel> DeserializeConfiguringFile(string path);
}