using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("maintenance", "assignment", "list")]
public record AzMaintenanceAssignmentListOptions(
[property: CliOption("--provider-name")] string ProviderName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--resource-name")] string ResourceName,
[property: CliOption("--resource-type")] string ResourceType
) : AzOptions;