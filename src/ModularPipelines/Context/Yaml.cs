using YamlDotNet.Serialization;

namespace ModularPipelines.Context;

internal class Yaml : IYaml
{
    public string ToYaml<T>(T input, INamingConvention namingConvention)
    {
        return new SerializerBuilder()
            .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
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