using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "secure-score-control-definitions", "list")]
public record AzSecuritySecureScoreControlDefinitionsListOptions : AzOptions
{
}

