using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("finspace", "update-kx-cluster-code-configuration")]
public record AwsFinspaceUpdateKxClusterCodeConfigurationOptions(
[property: CliOption("--environment-id")] string EnvironmentId,
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--code")] string Code
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--initialization-script")]
    public string? InitializationScript { get; set; }

    [CliOption("--command-line-arguments")]
    public string[]? CommandLineArguments { get; set; }

    [CliOption("--deployment-configuration")]
    public string? DeploymentConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}