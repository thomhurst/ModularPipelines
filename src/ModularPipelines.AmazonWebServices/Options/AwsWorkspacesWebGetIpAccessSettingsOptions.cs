using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces-web", "get-ip-access-settings")]
public record AwsWorkspacesWebGetIpAccessSettingsOptions(
[property: CommandSwitch("--ip-access-settings-arn")] string IpAccessSettingsArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}