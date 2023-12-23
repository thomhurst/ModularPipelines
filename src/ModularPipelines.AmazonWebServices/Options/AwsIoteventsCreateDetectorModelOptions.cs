using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotevents", "create-detector-model")]
public record AwsIoteventsCreateDetectorModelOptions(
[property: CommandSwitch("--detector-model-name")] string DetectorModelName,
[property: CommandSwitch("--detector-model-definition")] string DetectorModelDefinition,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--detector-model-description")]
    public string? DetectorModelDescription { get; set; }

    [CommandSwitch("--key")]
    public string? Key { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--evaluation-method")]
    public string? EvaluationMethod { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}