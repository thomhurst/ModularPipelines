using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("launch-wizard", "create-deployment")]
public record AwsLaunchWizardCreateDeploymentOptions(
[property: CliOption("--deployment-pattern-name")] string DeploymentPatternName,
[property: CliOption("--name")] string Name,
[property: CliOption("--specifications")] IEnumerable<KeyValue> Specifications,
[property: CliOption("--workload-name")] string WorkloadName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}