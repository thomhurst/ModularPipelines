using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "secure-scores", "show")]
public record AzSecuritySecureScoresShowOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
}

