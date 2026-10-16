using System.Text.Json.Serialization;

namespace ModularPipelines.Engine;

internal sealed record EstimatedTimeEntry(string ModuleType, string? SubModuleName, TimeSpan Duration);

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    RespectRequiredConstructorParameters = true,
    WriteIndented = true)]
[JsonSerializable(typeof(EstimatedTimeEntry))]
internal sealed partial class EstimatedTimeJsonContext : JsonSerializerContext;
