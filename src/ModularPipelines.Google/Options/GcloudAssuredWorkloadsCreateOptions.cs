using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("assured", "workloads", "create")]
public record GcloudAssuredWorkloadsCreateOptions(
[property: CommandSwitch("--billing-account")] string BillingAccount,
[property: CommandSwitch("--compliance-regime")] string ComplianceRegime,
[property: CommandSwitch("--display-name")] string DisplayName,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--organization")] string Organization
) : GcloudOptions
{
    [CommandSwitch("--enable-sovereign-controls")]
    public string? EnableSovereignControls { get; set; }

    [CommandSwitch("--external-identifier")]
    public string? ExternalIdentifier { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--next-rotation-time")]
    public string? NextRotationTime { get; set; }

    [CommandSwitch("--partner")]
    public string? Partner { get; set; }

    [CommandSwitch("--partner-permissions")]
    public IEnumerable<KeyValue>? PartnerPermissions { get; set; }

    [CommandSwitch("--provisioned-resources-parent")]
    public string? ProvisionedResourcesParent { get; set; }

    [CommandSwitch("--resource-settings")]
    public IEnumerable<KeyValue>? ResourceSettings { get; set; }

    [CommandSwitch("--rotation-period")]
    public string? RotationPeriod { get; set; }
}