using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ModularPipelines.Context;

public interface IYaml
{
    string ToYaml<T>(T input) => ToYaml(input, CamelCaseNamingConvention.Instance);
    string ToYaml<T>(T input, INamingConvention namingConvention);
    
    T FromYaml<T>(string input) => FromYaml<T>(input, CamelCaseNamingConvention.Instance);
    T FromYaml<T>(string input, INamingConvention namingConvention);
}