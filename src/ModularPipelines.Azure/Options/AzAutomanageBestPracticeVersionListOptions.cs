using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("automanage", "best-practice", "version", "list")]
public record AzAutomanageBestPracticeVersionListOptions(
[property: CliOption("--best-practice-name")] string BestPracticeName
) : AzOptions;