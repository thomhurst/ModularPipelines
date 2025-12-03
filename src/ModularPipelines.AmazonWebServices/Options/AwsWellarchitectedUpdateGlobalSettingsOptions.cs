using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wellarchitected", "update-global-settings")]
public record AwsWellarchitectedUpdateGlobalSettingsOptions : AwsOptions
{
    [CliOption("--organization-sharing-status")]
    public string? OrganizationSharingStatus { get; set; }

    [CliOption("--discovery-integration-status")]
    public string? DiscoveryIntegrationStatus { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}