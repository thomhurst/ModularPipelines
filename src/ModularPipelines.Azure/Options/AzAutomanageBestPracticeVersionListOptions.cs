using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("automanage", "best-practice", "version", "list")]
public record AzAutomanageBestPracticeVersionListOptions(
[property: CommandSwitch("--best-practice-name")] string BestPracticeName
) : AzOptions
{
}