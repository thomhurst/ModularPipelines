using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("automanage", "best-practice", "list")]
public record AzAutomanageBestPracticeListOptions : AzOptions;