using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "secure-scores", "show")]
public record AzSecuritySecureScoresShowOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;