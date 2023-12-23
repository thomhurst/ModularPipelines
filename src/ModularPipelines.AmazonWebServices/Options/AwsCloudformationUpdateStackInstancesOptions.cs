using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudformation", "update-stack-instances")]
public record AwsCloudformationUpdateStackInstancesOptions(
[property: CommandSwitch("--stack-set-name")] string StackSetName,
[property: CommandSwitch("--regions")] string[] Regions
) : AwsOptions
{
    [CommandSwitch("--accounts")]
    public string[]? Accounts { get; set; }

    [CommandSwitch("--deployment-targets")]
    public string? DeploymentTargets { get; set; }

    [CommandSwitch("--parameter-overrides")]
    public string[]? ParameterOverrides { get; set; }

    [CommandSwitch("--operation-preferences")]
    public string? OperationPreferences { get; set; }

    [CommandSwitch("--operation-id")]
    public string? OperationId { get; set; }

    [CommandSwitch("--call-as")]
    public string? CallAs { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}