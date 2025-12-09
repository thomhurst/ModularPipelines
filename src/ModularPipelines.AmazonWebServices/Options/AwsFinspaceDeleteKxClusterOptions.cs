using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("finspace", "delete-kx-cluster")]
public record AwsFinspaceDeleteKxClusterOptions(
[property: CliOption("--environment-id")] string EnvironmentId,
[property: CliOption("--cluster-name")] string ClusterName
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}