using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("finspace", "update-kx-cluster-databases")]
public record AwsFinspaceUpdateKxClusterDatabasesOptions(
[property: CommandSwitch("--environment-id")] string EnvironmentId,
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--databases")] string[] Databases
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--deployment-configuration")]
    public string? DeploymentConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}