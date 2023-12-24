using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("guardduty", "update-organization-configuration")]
public record AwsGuarddutyUpdateOrganizationConfigurationOptions(
[property: CommandSwitch("--detector-id")] string DetectorId
) : AwsOptions
{
    [CommandSwitch("--data-sources")]
    public string? DataSources { get; set; }

    [CommandSwitch("--features")]
    public string[]? Features { get; set; }

    [CommandSwitch("--auto-enable-organization-members")]
    public string? AutoEnableOrganizationMembers { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}