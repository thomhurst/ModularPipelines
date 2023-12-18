using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("automation", "software-update-configuration", "create")]
public record AzAutomationSoftwareUpdateConfigurationCreateOptions(
[property: CommandSwitch("--automation-account-name")] int AutomationAccountName,
[property: CommandSwitch("--configuration-name")] string ConfigurationName,
[property: CommandSwitch("--frequency")] string Frequency,
[property: CommandSwitch("--interval")] int Interval,
[property: CommandSwitch("--operating-system")] string OperatingSystem,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--azure-queries-locations")]
    public string? AzureQueriesLocations { get; set; }

    [CommandSwitch("--azure-queries-scope")]
    public string? AzureQueriesScope { get; set; }

    [CommandSwitch("--azure-queries-tags")]
    public string? AzureQueriesTags { get; set; }

    [CommandSwitch("--azure-virtual-machines")]
    public string? AzureVirtualMachines { get; set; }

    [CommandSwitch("--creation-time")]
    public string? CreationTime { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--duration")]
    public string? Duration { get; set; }

    [CommandSwitch("--excluded-kb-numbers")]
    public string? ExcludedKbNumbers { get; set; }

    [CommandSwitch("--expiry-time")]
    public string? ExpiryTime { get; set; }

    [CommandSwitch("--expiry-time-offset-minutes")]
    public string? ExpiryTimeOffsetMinutes { get; set; }

    [CommandSwitch("--included-kb-numbers")]
    public string? IncludedKbNumbers { get; set; }

    [CommandSwitch("--included-update-classifications")]
    public string? IncludedUpdateClassifications { get; set; }

    [BooleanCommandSwitch("--is-enabled")]
    public bool? IsEnabled { get; set; }

    [CommandSwitch("--last-modified-time")]
    public string? LastModifiedTime { get; set; }

    [CommandSwitch("--next-run")]
    public string? NextRun { get; set; }

    [CommandSwitch("--next-run-offset-minutes")]
    public string? NextRunOffsetMinutes { get; set; }

    [CommandSwitch("--non-azure-computer-names")]
    public string? NonAzureComputerNames { get; set; }

    [CommandSwitch("--non-azure-queries-function-alias")]
    public string? NonAzureQueriesFunctionAlias { get; set; }

    [CommandSwitch("--non-azure-queries-workspace-id")]
    public string? NonAzureQueriesWorkspaceId { get; set; }

    [CommandSwitch("--post-task-job-id")]
    public string? PostTaskJobId { get; set; }

    [CommandSwitch("--post-task-source")]
    public string? PostTaskSource { get; set; }

    [CommandSwitch("--post-task-status")]
    public string? PostTaskStatus { get; set; }

    [CommandSwitch("--pre-task-job-id")]
    public string? PreTaskJobId { get; set; }

    [CommandSwitch("--pre-task-source")]
    public string? PreTaskSource { get; set; }

    [CommandSwitch("--pre-task-status")]
    public string? PreTaskStatus { get; set; }

    [CommandSwitch("--reboot-setting")]
    public string? RebootSetting { get; set; }

    [CommandSwitch("--start-time")]
    public string? StartTime { get; set; }

    [CommandSwitch("--time-zone")]
    public string? TimeZone { get; set; }
}