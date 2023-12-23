using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces-web", "delete-ip-access-settings")]
public record AwsWorkspacesWebDeleteIpAccessSettingsOptions(
[property: CommandSwitch("--ip-access-settings-arn")] string IpAccessSettingsArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}