using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("functionapp", "deployment", "user", "show")]
public record AzFunctionappDeploymentUserShowOptions : AzOptions;