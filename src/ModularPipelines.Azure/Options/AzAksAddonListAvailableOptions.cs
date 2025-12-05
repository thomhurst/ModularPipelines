using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("aks", "addon", "list-available")]
public record AzAksAddonListAvailableOptions : AzOptions;