using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using ModularPipelines.Context;

namespace ModularPipelines.Context;

internal class Json : IJsonContext
{
    [RequiresDynamicCode("Reflection-based JSON serialization may require runtime code generation. Use the JsonTypeInfo overload for Native AOT.")]
    [RequiresUnreferencedCode("Reflection-based JSON serialization may require members that trimming cannot statically discover. Use the JsonTypeInfo overload when trimming.")]
    public string ToJson<T>(T input)
    {
        return JsonSerializer.Serialize(input);
    }

    [RequiresDynamicCode("Reflection-based JSON serialization may require runtime code generation. Use the JsonTypeInfo overload for Native AOT.")]
    [RequiresUnreferencedCode("Reflection-based JSON serialization may require members that trimming cannot statically discover. Use the JsonTypeInfo overload when trimming.")]
    public string ToJson<T>(T input, JsonSerializerOptions options)
    {
        return JsonSerializer.Serialize(input, options);
    }

    public string ToJson<T>(T input, JsonTypeInfo<T> jsonTypeInfo)
    {
        return JsonSerializer.Serialize(input, jsonTypeInfo);
    }

    [RequiresDynamicCode("Reflection-based JSON deserialization may require runtime code generation. Use the JsonTypeInfo overload for Native AOT.")]
    [RequiresUnreferencedCode("Reflection-based JSON deserialization may require members that trimming cannot statically discover. Use the JsonTypeInfo overload when trimming.")]
    public T? FromJson<T>(string input)
    {
        return JsonSerializer.Deserialize<T>(input);
    }

    [RequiresDynamicCode("Reflection-based JSON deserialization may require runtime code generation. Use the JsonTypeInfo overload for Native AOT.")]
    [RequiresUnreferencedCode("Reflection-based JSON deserialization may require members that trimming cannot statically discover. Use the JsonTypeInfo overload when trimming.")]
    public T? FromJson<T>(string input, JsonSerializerOptions options)
    {
        return JsonSerializer.Deserialize<T>(input, options);
    }

    public T? FromJson<T>(string input, JsonTypeInfo<T> jsonTypeInfo)
    {
        return JsonSerializer.Deserialize(input, jsonTypeInfo);
    }
}
