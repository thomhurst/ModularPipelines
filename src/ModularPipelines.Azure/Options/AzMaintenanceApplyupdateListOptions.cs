using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("maintenance", "applyupdate", "list")]
public record AzMaintenanceApplyupdateListOptions : AzOptions;