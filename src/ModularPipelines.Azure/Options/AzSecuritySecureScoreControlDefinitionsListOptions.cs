using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "secure-score-control-definitions", "list")]
public record AzSecuritySecureScoreControlDefinitionsListOptions : AzOptions;