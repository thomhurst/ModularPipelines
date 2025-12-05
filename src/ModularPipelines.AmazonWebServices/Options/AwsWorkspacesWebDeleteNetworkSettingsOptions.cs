using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces-web", "delete-network-settings")]
public record AwsWorkspacesWebDeleteNetworkSettingsOptions(
[property: CliOption("--network-settings-arn")] string NetworkSettingsArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}