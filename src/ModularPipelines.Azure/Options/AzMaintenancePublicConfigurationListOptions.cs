using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("maintenance", "public-configuration", "list")]
public record AzMaintenancePublicConfigurationListOptions : AzOptions;