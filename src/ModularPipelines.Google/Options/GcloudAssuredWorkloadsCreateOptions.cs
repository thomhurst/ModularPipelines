using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("assured", "workloads", "create")]
public record GcloudAssuredWorkloadsCreateOptions(
[property: CliOption("--billing-account")] string BillingAccount,
[property: CliOption("--compliance-regime")] string ComplianceRegime,
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--location")] string Location,
[property: CliOption("--organization")] string Organization
) : GcloudOptions
{
    [CliOption("--enable-sovereign-controls")]
    public string? EnableSovereignControls { get; set; }

    [CliOption("--external-identifier")]
    public string? ExternalIdentifier { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--next-rotation-time")]
    public string? NextRotationTime { get; set; }

    [CliOption("--partner")]
    public string? Partner { get; set; }

    [CliOption("--partner-permissions")]
    public IEnumerable<KeyValue>? PartnerPermissions { get; set; }

    [CliOption("--provisioned-resources-parent")]
    public string? ProvisionedResourcesParent { get; set; }

    [CliOption("--resource-settings")]
    public IEnumerable<KeyValue>? ResourceSettings { get; set; }

    [CliOption("--rotation-period")]
    public string? RotationPeriod { get; set; }
}