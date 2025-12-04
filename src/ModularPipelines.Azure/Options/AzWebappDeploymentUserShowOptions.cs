using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("webapp", "deployment", "user", "show")]
public record AzWebappDeploymentUserShowOptions : AzOptions;