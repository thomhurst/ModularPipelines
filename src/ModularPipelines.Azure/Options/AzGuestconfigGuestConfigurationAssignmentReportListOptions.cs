using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("guestconfig", "guest-configuration-assignment-report", "list")]
public record AzGuestconfigGuestConfigurationAssignmentReportListOptions(
[property: CliOption("--guest-configuration-assignment-name")] string GuestConfigurationAssignmentName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vm-name")] string VmName
) : AzOptions;