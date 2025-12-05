using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "list-deployment-targets")]
public record AwsDeployListDeploymentTargetsOptions(
[property: CliOption("--deployment-id")] string DeploymentId
) : AwsOptions
{
    [CliOption("--target-filters")]
    public IEnumerable<KeyValue>? TargetFilters { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}