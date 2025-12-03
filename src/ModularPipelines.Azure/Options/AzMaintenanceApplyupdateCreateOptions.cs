using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("maintenance", "applyupdate", "create")]
public record AzMaintenanceApplyupdateCreateOptions(
[property: CliOption("--provider-name")] string ProviderName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--resource-name")] string ResourceName,
[property: CliOption("--resource-type")] string ResourceType
) : AzOptions;