using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("support", "services", "problem-classifications", "show")]
public record AzSupportServicesProblemClassificationsShowOptions(
[property: CommandSwitch("--problem-classification-name")] string ProblemClassificationName,
[property: CommandSwitch("--service-name")] string ServiceName
) : AzOptions
{
}

