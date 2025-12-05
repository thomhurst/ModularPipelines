using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("guestconfig", "guest-configuration-hcrp-assignment", "list")]
public record AzGuestconfigGuestConfigurationHcrpAssignmentListOptions(
[property: CliOption("--machine-name")] string MachineName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;