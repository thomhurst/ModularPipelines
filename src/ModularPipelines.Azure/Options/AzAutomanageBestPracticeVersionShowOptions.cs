using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("automanage", "best-practice", "version", "show")]
public record AzAutomanageBestPracticeVersionShowOptions(
[property: CliOption("--best-practice-name")] string BestPracticeName,
[property: CliOption("--version-name")] string VersionName
) : AzOptions;