using System.Text.Json;

namespace ModularPipelines.Context;

internal class Json : IJson
{
    public string ToJson<T>(T input)
    {
        return JsonSerializer.Serialize(input);
    }

    public string ToJson<T>(T input, JsonSerializerOptions options)
    {
        return JsonSerializer.Serialize(input, options);
    }

    public T? FromJson<T>(string input)
    {
        return JsonSerializer.Deserialize<T>(input);
    }

    public T? FromJson<T>(string input, JsonSerializerOptions options)
    {
        return JsonSerializer.Deserialize<T>(input, options);
    }
}
