using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("automanage", "best-practice", "show")]
public record AzAutomanageBestPracticeShowOptions(
[property: CommandSwitch("--best-practice-name")] string BestPracticeName
) : AzOptions;