using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("support", "services", "show")]
public record AzSupportServicesShowOptions(
[property: CliOption("--service-name")] string ServiceName
) : AzOptions;