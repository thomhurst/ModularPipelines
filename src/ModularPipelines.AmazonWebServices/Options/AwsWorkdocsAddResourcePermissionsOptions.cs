using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workdocs", "add-resource-permissions")]
public record AwsWorkdocsAddResourcePermissionsOptions(
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--principals")] string[] Principals
) : AwsOptions
{
    [CommandSwitch("--authentication-token")]
    public string? AuthenticationToken { get; set; }

    [CommandSwitch("--notification-options")]
    public string? NotificationOptions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}