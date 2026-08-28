using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using YamlDotNet.Serialization;

namespace ModularPipelines.Context;

internal class Yaml : IYamlContext
{
    [RequiresDynamicCode("YamlDotNet serialization may require runtime code generation.")]
    [RequiresUnreferencedCode("YamlDotNet serialization uses reflection over members that may be removed by trimming.")]
    public string ToYaml<T>(T input, INamingConvention namingConvention)
    {
        return new SerializerBuilder()
            .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
            .WithNamingConvention(namingConvention)
            .Build()
            .Serialize(input);
    }

    [RequiresDynamicCode("YamlDotNet deserialization may require runtime code generation.")]
    [RequiresUnreferencedCode("YamlDotNet deserialization uses reflection over members that may be removed by trimming.")]
    public T FromYaml<T>(string input, INamingConvention namingConvention)
    {
        return new DeserializerBuilder()
            .WithNamingConvention(namingConvention)
            .Build()
            .Deserialize<T>(input);
    }
}
