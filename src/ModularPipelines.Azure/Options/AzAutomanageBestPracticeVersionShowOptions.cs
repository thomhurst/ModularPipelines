using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("automanage", "best-practice", "version", "show")]
public record AzAutomanageBestPracticeVersionShowOptions(
[property: CommandSwitch("--best-practice-name")] string BestPracticeName,
[property: CommandSwitch("--version-name")] string VersionName
) : AzOptions
{
}