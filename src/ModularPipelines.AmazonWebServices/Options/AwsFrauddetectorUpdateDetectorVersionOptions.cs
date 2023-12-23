using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("frauddetector", "update-detector-version")]
public record AwsFrauddetectorUpdateDetectorVersionOptions(
[property: CommandSwitch("--detector-id")] string DetectorId,
[property: CommandSwitch("--detector-version-id")] string DetectorVersionId,
[property: CommandSwitch("--external-model-endpoints")] string[] ExternalModelEndpoints,
[property: CommandSwitch("--rules")] string[] Rules
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--model-versions")]
    public string[]? ModelVersions { get; set; }

    [CommandSwitch("--rule-execution-mode")]
    public string? RuleExecutionMode { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}