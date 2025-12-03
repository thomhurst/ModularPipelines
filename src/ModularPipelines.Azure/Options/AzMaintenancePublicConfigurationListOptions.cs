using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("maintenance", "public-configuration", "list")]
public record AzMaintenancePublicConfigurationListOptions : AzOptions;