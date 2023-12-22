using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("support", "services", "problem-classifications", "show")]
public record AzSupportServicesProblemClassificationsShowOptions(
[property: CommandSwitch("--problem-classification-name")] string ProblemClassificationName,
[property: CommandSwitch("--service-name")] string ServiceName
) : AzOptions;