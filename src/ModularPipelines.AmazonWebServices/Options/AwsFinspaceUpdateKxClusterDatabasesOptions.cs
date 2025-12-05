using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("finspace", "update-kx-cluster-databases")]
public record AwsFinspaceUpdateKxClusterDatabasesOptions(
[property: CliOption("--environment-id")] string EnvironmentId,
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--databases")] string[] Databases
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--deployment-configuration")]
    public string? DeploymentConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}