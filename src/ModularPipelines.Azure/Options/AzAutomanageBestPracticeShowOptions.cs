using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("automanage", "best-practice", "show")]
public record AzAutomanageBestPracticeShowOptions(
[property: CliOption("--best-practice-name")] string BestPracticeName
) : AzOptions;