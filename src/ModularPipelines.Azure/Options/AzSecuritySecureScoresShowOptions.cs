using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("security", "secure-scores", "show")]
public record AzSecuritySecureScoresShowOptions(
[property: CliOption("--name")] string Name
) : AzOptions;