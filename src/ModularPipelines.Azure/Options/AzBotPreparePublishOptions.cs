using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bot", "prepare-publish")]
public record AzBotPreparePublishOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--proj-file-path")] string ProjFilePath,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--sln-name")] string SlnName
) : AzOptions
{
    [CommandSwitch("--code-dir")]
    public string? CodeDir { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }
}