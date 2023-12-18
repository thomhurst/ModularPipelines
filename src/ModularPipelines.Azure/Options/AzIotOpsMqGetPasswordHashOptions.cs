using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "ops", "mq", "get-password-hash")]
public record AzIotOpsMqGetPasswordHashOptions(
[property: CommandSwitch("--phrase")] string Phrase
) : AzOptions
{
    [CommandSwitch("--iterations")]
    public string? Iterations { get; set; }
}

