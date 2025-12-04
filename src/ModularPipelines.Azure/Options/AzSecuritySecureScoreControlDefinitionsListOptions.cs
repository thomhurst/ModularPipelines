using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "secure-score-control-definitions", "list")]
public record AzSecuritySecureScoreControlDefinitionsListOptions : AzOptions;