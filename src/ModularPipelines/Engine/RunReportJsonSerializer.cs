using System.Text.Json;
using System.Text.Json.Serialization;
using ModularPipelines.Models;

namespace ModularPipelines.Engine;

internal static class RunReportJsonSerializer
{
    public static string Serialize(PipelineRunReport report) =>
        JsonSerializer.Serialize(report, RunReportJsonContext.Default.PipelineRunReport);

    public static PipelineRunReport? Deserialize(string json) =>
        JsonSerializer.Deserialize(json, RunReportJsonContext.Default.PipelineRunReport);
}

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    WriteIndented = true)]
[JsonSerializable(typeof(PipelineRunReport))]
internal sealed partial class RunReportJsonContext : JsonSerializerContext;
