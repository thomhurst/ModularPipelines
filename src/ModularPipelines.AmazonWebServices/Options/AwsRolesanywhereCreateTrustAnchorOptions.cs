using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rolesanywhere", "create-trust-anchor")]
public record AwsRolesanywhereCreateTrustAnchorOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--source")] string Source
) : AwsOptions
{
    [CommandSwitch("--notification-settings")]
    public string[]? NotificationSettings { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}