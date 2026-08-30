using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace ModularPipelines.Context;

/// <summary>
/// Provides JSON serialization and deserialization functionality.
/// </summary>
public interface IJsonContext
{
    /// <summary>
    /// Serializes an object to JSON string using default options.
    /// </summary>
    /// <typeparam name="T">The type of object to serialize.</typeparam>
    /// <param name="input">The object to serialize.</param>
    /// <returns>The JSON string representation of the object.</returns>
    [RequiresDynamicCode("Reflection-based JSON serialization may require runtime code generation. Use the JsonTypeInfo overload for Native AOT.")]
    [RequiresUnreferencedCode("Reflection-based JSON serialization may require members that trimming cannot statically discover. Use the JsonTypeInfo overload when trimming.")]
    string ToJson<T>(T input);

    /// <summary>
    /// Serializes an object to JSON string using the specified options.
    /// </summary>
    /// <typeparam name="T">The type of object to serialize.</typeparam>
    /// <param name="input">The object to serialize.</param>
    /// <param name="options">The JSON serializer options to use.</param>
    /// <returns>The JSON string representation of the object.</returns>
    [RequiresDynamicCode("Reflection-based JSON serialization may require runtime code generation. Use the JsonTypeInfo overload for Native AOT.")]
    [RequiresUnreferencedCode("Reflection-based JSON serialization may require members that trimming cannot statically discover. Use the JsonTypeInfo overload when trimming.")]
    string ToJson<T>(T input, JsonSerializerOptions options);

    /// <summary>
    /// Serializes an object using source-generated JSON metadata.
    /// </summary>
    /// <typeparam name="T">The type of object to serialize.</typeparam>
    /// <param name="input">The object to serialize.</param>
    /// <param name="jsonTypeInfo">The source-generated JSON metadata to use.</param>
    /// <returns>The JSON string representation of the object.</returns>
    string ToJson<T>(T input, JsonTypeInfo<T> jsonTypeInfo);

    /// <summary>
    /// Deserializes a JSON string to an object using default options.
    /// </summary>
    /// <typeparam name="T">The type to deserialize to.</typeparam>
    /// <param name="input">The JSON string to deserialize.</param>
    /// <returns>The deserialized object, or null if deserialization fails.</returns>
    [RequiresDynamicCode("Reflection-based JSON deserialization may require runtime code generation. Use the JsonTypeInfo overload for Native AOT.")]
    [RequiresUnreferencedCode("Reflection-based JSON deserialization may require members that trimming cannot statically discover. Use the JsonTypeInfo overload when trimming.")]
    T? FromJson<T>(string input);

    /// <summary>
    /// Deserializes a JSON string to an object using the specified options.
    /// </summary>
    /// <typeparam name="T">The type to deserialize to.</typeparam>
    /// <param name="input">The JSON string to deserialize.</param>
    /// <param name="options">The JSON serializer options to use.</param>
    /// <returns>The deserialized object, or null if deserialization fails.</returns>
    [RequiresDynamicCode("Reflection-based JSON deserialization may require runtime code generation. Use the JsonTypeInfo overload for Native AOT.")]
    [RequiresUnreferencedCode("Reflection-based JSON deserialization may require members that trimming cannot statically discover. Use the JsonTypeInfo overload when trimming.")]
    T? FromJson<T>(string input, JsonSerializerOptions options);

    /// <summary>
    /// Deserializes JSON using source-generated JSON metadata.
    /// </summary>
    /// <typeparam name="T">The type to deserialize to.</typeparam>
    /// <param name="input">The JSON string to deserialize.</param>
    /// <param name="jsonTypeInfo">The source-generated JSON metadata to use.</param>
    /// <returns>The deserialized object, or null if deserialization fails.</returns>
    T? FromJson<T>(string input, JsonTypeInfo<T> jsonTypeInfo);
}
