using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bot", "prepare-deploy")]
public record AzBotPrepareDeployOptions(
[property: CommandSwitch("--lang")] string Lang
) : AzOptions
{
    [CommandSwitch("--code-dir")]
    public string? CodeDir { get; set; }

    [CommandSwitch("--proj-file-path")]
    public string? ProjFilePath { get; set; }
}