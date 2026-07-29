using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context.Domains.Data;
using YamlDotNet.Serialization;

namespace ModularPipelines.Context;

internal class Yaml : IYamlContext
{
    [RequiresDynamicCode("YamlDotNet serialization may require runtime code generation.")]
    public string ToYaml<T>(T input, INamingConvention namingConvention)
    {
        return new SerializerBuilder()
            .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
            .WithNamingConvention(namingConvention)
            .Build()
            .Serialize(input);
    }

    [RequiresDynamicCode("YamlDotNet deserialization may require runtime code generation.")]
    public T FromYaml<T>(string input, INamingConvention namingConvention)
    {
        return new DeserializerBuilder()
            .WithNamingConvention(namingConvention)
            .Build()
            .Deserialize<T>(input);
    }
}
