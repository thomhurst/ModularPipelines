using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("webapp", "deployment", "user", "show")]
public record AzWebappDeploymentUserShowOptions : AzOptions;