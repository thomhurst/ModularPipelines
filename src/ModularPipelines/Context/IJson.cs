using System.Text.Json;

namespace ModularPipelines.Context;

/// <summary>
/// Provides JSON serialization and deserialization functionality.
/// </summary>
public interface IJson
{
    /// <summary>
    /// Serializes an object to JSON string using default options.
    /// </summary>
    /// <typeparam name="T">The type of object to serialize.</typeparam>
    /// <param name="input">The object to serialize.</param>
    /// <returns>The JSON string representation of the object.</returns>
    string ToJson<T>(T input);

    /// <summary>
    /// Serializes an object to JSON string using the specified options.
    /// </summary>
    /// <typeparam name="T">The type of object to serialize.</typeparam>
    /// <param name="input">The object to serialize.</param>
    /// <param name="options">The JSON serializer options to use.</param>
    /// <returns>The JSON string representation of the object.</returns>
    string ToJson<T>(T input, JsonSerializerOptions options);

    /// <summary>
    /// Deserializes a JSON string to an object using default options.
    /// </summary>
    /// <typeparam name="T">The type to deserialize to.</typeparam>
    /// <param name="input">The JSON string to deserialize.</param>
    /// <returns>The deserialized object, or null if deserialization fails.</returns>
    T? FromJson<T>(string input);

    /// <summary>
    /// Deserializes a JSON string to an object using the specified options.
    /// </summary>
    /// <typeparam name="T">The type to deserialize to.</typeparam>
    /// <param name="input">The JSON string to deserialize.</param>
    /// <param name="options">The JSON serializer options to use.</param>
    /// <returns>The deserialized object, or null if deserialization fails.</returns>
    T? FromJson<T>(string input, JsonSerializerOptions options);
}
