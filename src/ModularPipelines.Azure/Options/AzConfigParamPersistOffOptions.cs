using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("config", "param-persist", "off")]
public record AzConfigParamPersistOffOptions : AzOptions;