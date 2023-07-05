using YamlDotNet.Serialization;

namespace ModularPipelines.Context;

public class Yaml : IYaml
{
    public string ToYaml<T>(T input, INamingConvention namingConvention)
    {
        return new SerializerBuilder()
            .WithNamingConvention(namingConvention)
            .Build()
            .Serialize(input);
    }

    public T FromYaml<T>(string input, INamingConvention namingConvention)
    {
        return new DeserializerBuilder()
            .WithNamingConvention(namingConvention)
            .Build()
            .Deserialize<T>(input);
    }
}
