using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("guardduty", "update-organization-configuration")]
public record AwsGuarddutyUpdateOrganizationConfigurationOptions(
[property: CliOption("--detector-id")] string DetectorId
) : AwsOptions
{
    [CliOption("--data-sources")]
    public string? DataSources { get; set; }

    [CliOption("--features")]
    public string[]? Features { get; set; }

    [CliOption("--auto-enable-organization-members")]
    public string? AutoEnableOrganizationMembers { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}