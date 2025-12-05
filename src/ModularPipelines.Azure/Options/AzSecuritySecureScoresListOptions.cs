using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "secure-scores", "list")]
public record AzSecuritySecureScoresListOptions : AzOptions;