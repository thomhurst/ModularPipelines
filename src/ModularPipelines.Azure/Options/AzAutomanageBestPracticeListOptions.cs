using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("automanage", "best-practice", "list")]
public record AzAutomanageBestPracticeListOptions(
[property: CommandSwitch("--best-practice-name")] string BestPracticeName
) : AzOptions
{
}