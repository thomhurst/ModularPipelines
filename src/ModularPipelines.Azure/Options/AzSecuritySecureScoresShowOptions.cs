using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "secure-scores", "show")]
public record AzSecuritySecureScoresShowOptions(
[property: CliOption("--name")] string Name
) : AzOptions;