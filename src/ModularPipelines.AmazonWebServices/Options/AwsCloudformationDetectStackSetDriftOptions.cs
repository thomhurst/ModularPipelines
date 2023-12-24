using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudformation", "detect-stack-set-drift")]
public record AwsCloudformationDetectStackSetDriftOptions(
[property: CommandSwitch("--stack-set-name")] string StackSetName
) : AwsOptions
{
    [CommandSwitch("--operation-preferences")]
    public string? OperationPreferences { get; set; }

    [CommandSwitch("--operation-id")]
    public string? OperationId { get; set; }

    [CommandSwitch("--call-as")]
    public string? CallAs { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}