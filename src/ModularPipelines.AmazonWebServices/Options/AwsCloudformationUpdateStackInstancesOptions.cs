using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "update-stack-instances")]
public record AwsCloudformationUpdateStackInstancesOptions(
[property: CliOption("--stack-set-name")] string StackSetName,
[property: CliOption("--regions")] string[] Regions
) : AwsOptions
{
    [CliOption("--accounts")]
    public string[]? Accounts { get; set; }

    [CliOption("--deployment-targets")]
    public string? DeploymentTargets { get; set; }

    [CliOption("--parameter-overrides")]
    public string[]? ParameterOverrides { get; set; }

    [CliOption("--operation-preferences")]
    public string? OperationPreferences { get; set; }

    [CliOption("--operation-id")]
    public string? OperationId { get; set; }

    [CliOption("--call-as")]
    public string? CallAs { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}