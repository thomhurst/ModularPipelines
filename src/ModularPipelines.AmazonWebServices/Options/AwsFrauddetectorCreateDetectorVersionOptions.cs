using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("frauddetector", "create-detector-version")]
public record AwsFrauddetectorCreateDetectorVersionOptions(
[property: CommandSwitch("--detector-id")] string DetectorId,
[property: CommandSwitch("--rules")] string[] Rules
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--external-model-endpoints")]
    public string[]? ExternalModelEndpoints { get; set; }

    [CommandSwitch("--model-versions")]
    public string[]? ModelVersions { get; set; }

    [CommandSwitch("--rule-execution-mode")]
    public string? RuleExecutionMode { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}