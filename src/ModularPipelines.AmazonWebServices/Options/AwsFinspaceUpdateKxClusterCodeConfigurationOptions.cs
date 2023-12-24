using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("finspace", "update-kx-cluster-code-configuration")]
public record AwsFinspaceUpdateKxClusterCodeConfigurationOptions(
[property: CommandSwitch("--environment-id")] string EnvironmentId,
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--code")] string Code
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--initialization-script")]
    public string? InitializationScript { get; set; }

    [CommandSwitch("--command-line-arguments")]
    public string[]? CommandLineArguments { get; set; }

    [CommandSwitch("--deployment-configuration")]
    public string? DeploymentConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}