using Deserialize.Exceptions;
using Deserialize.YamlDeserialize;
using YamlDotNet.Serialization.NamingConventions;

namespace Deserialize;

public static class Deserializer<TModel>
    where TModel : class
{
    public static TModel Deserialize(string path)
        => GetFileExtensions(path) switch
        {
            ".yml" => new DeserializerYaml<TModel>().DeserializeConfiguringFile(path, PascalCaseNamingConvention.Instance),
            //".json" => null,
            //".xml" => null,
            _ => throw new UnknowFileExtensionException("Не допустимый формат файла")
        };

    private static string GetFileExtensions(string path)
        => path[path.IndexOf(".")..];
}