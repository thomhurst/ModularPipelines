using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("support", "services", "problem-classifications", "list")]
public record AzSupportServicesProblemClassificationsListOptions(
[property: CliOption("--service-name")] string ServiceName
) : AzOptions;