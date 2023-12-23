using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wellarchitected", "update-global-settings")]
public record AwsWellarchitectedUpdateGlobalSettingsOptions : AwsOptions
{
    [CommandSwitch("--organization-sharing-status")]
    public string? OrganizationSharingStatus { get; set; }

    [CommandSwitch("--discovery-integration-status")]
    public string? DiscoveryIntegrationStatus { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}