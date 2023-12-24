using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("organizations", "invite-account-to-organization")]
public record AwsOrganizationsInviteAccountToOrganizationOptions(
[property: CommandSwitch("--target")] string Target
) : AwsOptions
{
    [CommandSwitch("--notes")]
    public string? Notes { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}