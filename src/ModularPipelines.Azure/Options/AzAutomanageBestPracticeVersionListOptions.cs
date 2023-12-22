using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("automanage", "best-practice", "version", "list")]
public record AzAutomanageBestPracticeVersionListOptions(
[property: CommandSwitch("--best-practice-name")] string BestPracticeName
) : AzOptions;