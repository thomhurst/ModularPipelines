using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acr", "config", "soft-delete", "show")]
public record AzAcrConfigSoftDeleteShowOptions(
[property: CliOption("--registry")] string Registry
) : AzOptions;