using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "get-plan")]
public record AwsGlueGetPlanOptions(
[property: CommandSwitch("--mapping")] string[] Mapping,
[property: CommandSwitch("--source")] string Source
) : AwsOptions
{
    [CommandSwitch("--sinks")]
    public string[]? Sinks { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--language")]
    public string? Language { get; set; }

    [CommandSwitch("--additional-plan-options-map")]
    public IEnumerable<KeyValue>? AdditionalPlanOptionsMap { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}