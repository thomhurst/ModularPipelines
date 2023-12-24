using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("frauddetector", "update-model-version")]
public record AwsFrauddetectorUpdateModelVersionOptions(
[property: CommandSwitch("--model-id")] string ModelId,
[property: CommandSwitch("--model-type")] string ModelType,
[property: CommandSwitch("--major-version-number")] string MajorVersionNumber
) : AwsOptions
{
    [CommandSwitch("--external-events-detail")]
    public string? ExternalEventsDetail { get; set; }

    [CommandSwitch("--ingested-events-detail")]
    public string? IngestedEventsDetail { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}