using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("support", "services", "problem-classifications", "show")]
public record AzSupportServicesProblemClassificationsShowOptions(
[property: CliOption("--problem-classification-name")] string ProblemClassificationName,
[property: CliOption("--service-name")] string ServiceName
) : AzOptions;