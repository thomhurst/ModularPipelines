using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("automation", "software-update-configuration", "create")]
public record AzAutomationSoftwareUpdateConfigurationCreateOptions(
[property: CliOption("--automation-account-name")] int AutomationAccountName,
[property: CliOption("--configuration-name")] string ConfigurationName,
[property: CliOption("--frequency")] string Frequency,
[property: CliOption("--interval")] int Interval,
[property: CliOption("--operating-system")] string OperatingSystem,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--azure-queries-locations")]
    public string? AzureQueriesLocations { get; set; }

    [CliOption("--azure-queries-scope")]
    public string? AzureQueriesScope { get; set; }

    [CliOption("--azure-queries-tags")]
    public string? AzureQueriesTags { get; set; }

    [CliOption("--azure-virtual-machines")]
    public string? AzureVirtualMachines { get; set; }

    [CliOption("--creation-time")]
    public string? CreationTime { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--duration")]
    public string? Duration { get; set; }

    [CliOption("--excluded-kb-numbers")]
    public string? ExcludedKbNumbers { get; set; }

    [CliOption("--expiry-time")]
    public string? ExpiryTime { get; set; }

    [CliOption("--expiry-time-offset-minutes")]
    public string? ExpiryTimeOffsetMinutes { get; set; }

    [CliOption("--included-kb-numbers")]
    public string? IncludedKbNumbers { get; set; }

    [CliOption("--included-update-classifications")]
    public string? IncludedUpdateClassifications { get; set; }

    [CliFlag("--is-enabled")]
    public bool? IsEnabled { get; set; }

    [CliOption("--last-modified-time")]
    public string? LastModifiedTime { get; set; }

    [CliOption("--next-run")]
    public string? NextRun { get; set; }

    [CliOption("--next-run-offset-minutes")]
    public string? NextRunOffsetMinutes { get; set; }

    [CliOption("--non-azure-computer-names")]
    public string? NonAzureComputerNames { get; set; }

    [CliOption("--non-azure-queries-function-alias")]
    public string? NonAzureQueriesFunctionAlias { get; set; }

    [CliOption("--non-azure-queries-workspace-id")]
    public string? NonAzureQueriesWorkspaceId { get; set; }

    [CliOption("--post-task-job-id")]
    public string? PostTaskJobId { get; set; }

    [CliOption("--post-task-source")]
    public string? PostTaskSource { get; set; }

    [CliOption("--post-task-status")]
    public string? PostTaskStatus { get; set; }

    [CliOption("--pre-task-job-id")]
    public string? PreTaskJobId { get; set; }

    [CliOption("--pre-task-source")]
    public string? PreTaskSource { get; set; }

    [CliOption("--pre-task-status")]
    public string? PreTaskStatus { get; set; }

    [CliOption("--reboot-setting")]
    public string? RebootSetting { get; set; }

    [CliOption("--start-time")]
    public string? StartTime { get; set; }

    [CliOption("--time-zone")]
    public string? TimeZone { get; set; }
}