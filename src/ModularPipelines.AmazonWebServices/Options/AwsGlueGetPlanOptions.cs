using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "get-plan")]
public record AwsGlueGetPlanOptions(
[property: CliOption("--mapping")] string[] Mapping,
[property: CliOption("--source")] string Source
) : AwsOptions
{
    [CliOption("--sinks")]
    public string[]? Sinks { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--language")]
    public string? Language { get; set; }

    [CliOption("--additional-plan-options-map")]
    public IEnumerable<KeyValue>? AdditionalPlanOptionsMap { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}