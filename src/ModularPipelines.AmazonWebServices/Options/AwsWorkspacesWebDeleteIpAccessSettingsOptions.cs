using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces-web", "delete-ip-access-settings")]
public record AwsWorkspacesWebDeleteIpAccessSettingsOptions(
[property: CliOption("--ip-access-settings-arn")] string IpAccessSettingsArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}