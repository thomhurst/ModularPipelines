using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("guestconfig", "guest-configuration-hcrp-assignment-report", "list")]
public record AzGuestconfigGuestConfigurationHcrpAssignmentReportListOptions(
[property: CliOption("--guest-configuration-assignment-name")] string GuestConfigurationAssignmentName,
[property: CliOption("--machine-name")] string MachineName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;