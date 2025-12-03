using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("guestconfig", "guest-configuration-hcrp-assignment-report", "list")]
public record AzGuestconfigGuestConfigurationHcrpAssignmentReportListOptions(
[property: CliOption("--guest-configuration-assignment-name")] string GuestConfigurationAssignmentName,
[property: CliOption("--machine-name")] string MachineName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;