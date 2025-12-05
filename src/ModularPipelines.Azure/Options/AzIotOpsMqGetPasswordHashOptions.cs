using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "ops", "mq", "get-password-hash")]
public record AzIotOpsMqGetPasswordHashOptions(
[property: CliOption("--phrase")] string Phrase
) : AzOptions
{
    [CliOption("--iterations")]
    public string? Iterations { get; set; }
}