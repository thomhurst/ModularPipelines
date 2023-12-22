using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "secure-score-controls", "list")]
public record AzSecuritySecureScoreControlsListOptions : AzOptions;