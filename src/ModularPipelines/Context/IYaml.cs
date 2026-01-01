using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ModularPipelines.Context;

/// <summary>
/// Provides YAML serialization and deserialization functionality.
/// </summary>
public interface IYaml
{
    /// <summary>
    /// Serializes an object to YAML string using the default camel case naming convention.
    /// </summary>
    /// <typeparam name="T">The type of object to serialize.</typeparam>
    /// <param name="input">The object to serialize.</param>
    /// <returns>The YAML string representation of the object.</returns>
    string ToYaml<T>(T input) => ToYaml(input, CamelCaseNamingConvention.Instance);

    /// <summary>
    /// Serializes an object to YAML string using the specified naming convention.
    /// </summary>
    /// <typeparam name="T">The type of object to serialize.</typeparam>
    /// <param name="input">The object to serialize.</param>
    /// <param name="namingConvention">The naming convention to use for property names.</param>
    /// <returns>The YAML string representation of the object.</returns>
    string ToYaml<T>(T input, INamingConvention namingConvention);

    /// <summary>
    /// Deserializes a YAML string to an object using the default camel case naming convention.
    /// </summary>
    /// <typeparam name="T">The type to deserialize to.</typeparam>
    /// <param name="input">The YAML string to deserialize.</param>
    /// <returns>The deserialized object.</returns>
    T FromYaml<T>(string input) => FromYaml<T>(input, CamelCaseNamingConvention.Instance);

    /// <summary>
    /// Deserializes a YAML string to an object using the specified naming convention.
    /// </summary>
    /// <typeparam name="T">The type to deserialize to.</typeparam>
    /// <param name="input">The YAML string to deserialize.</param>
    /// <param name="namingConvention">The naming convention to use for property names.</param>
    /// <returns>The deserialized object.</returns>
    T FromYaml<T>(string input, INamingConvention namingConvention);
}
