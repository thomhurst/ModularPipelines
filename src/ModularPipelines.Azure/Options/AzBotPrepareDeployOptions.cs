using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bot", "prepare-deploy")]
public record AzBotPrepareDeployOptions(
[property: CliOption("--lang")] string Lang
) : AzOptions
{
    [CliOption("--code-dir")]
    public string? CodeDir { get; set; }

    [CliOption("--proj-file-path")]
    public string? ProjFilePath { get; set; }
}