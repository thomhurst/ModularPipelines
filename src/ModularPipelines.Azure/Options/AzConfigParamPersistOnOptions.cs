using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("config", "param-persist", "on")]
public record AzConfigParamPersistOnOptions : AzOptions;