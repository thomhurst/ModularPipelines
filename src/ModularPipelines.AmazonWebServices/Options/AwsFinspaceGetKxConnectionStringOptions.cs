using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("finspace", "get-kx-connection-string")]
public record AwsFinspaceGetKxConnectionStringOptions(
[property: CommandSwitch("--user-arn")] string UserArn,
[property: CommandSwitch("--environment-id")] string EnvironmentId,
[property: CommandSwitch("--cluster-name")] string ClusterName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}