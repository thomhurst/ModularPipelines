using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("finspace", "delete-kx-cluster")]
public record AwsFinspaceDeleteKxClusterOptions(
[property: CommandSwitch("--environment-id")] string EnvironmentId,
[property: CommandSwitch("--cluster-name")] string ClusterName
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}