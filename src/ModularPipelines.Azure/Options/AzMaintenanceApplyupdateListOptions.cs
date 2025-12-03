using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("maintenance", "applyupdate", "list")]
public record AzMaintenanceApplyupdateListOptions : AzOptions;