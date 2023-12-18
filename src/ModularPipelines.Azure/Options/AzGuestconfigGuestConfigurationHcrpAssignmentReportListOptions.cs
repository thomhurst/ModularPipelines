using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("guestconfig", "guest-configuration-hcrp-assignment-report", "list")]
public record AzGuestconfigGuestConfigurationHcrpAssignmentReportListOptions(
[property: CommandSwitch("--guest-configuration-assignment-name")] string GuestConfigurationAssignmentName,
[property: CommandSwitch("--machine-name")] string MachineName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;