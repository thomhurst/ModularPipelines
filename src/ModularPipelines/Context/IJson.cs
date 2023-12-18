using System.Text.Json;

namespace ModularPipelines.Context;

public interface IJson
{
    string ToJson<T>(T input);

    string ToJson<T>(T input, JsonSerializerOptions options);

    T? FromJson<T>(string input);

    T? FromJson<T>(string input, JsonSerializerOptions options);
}