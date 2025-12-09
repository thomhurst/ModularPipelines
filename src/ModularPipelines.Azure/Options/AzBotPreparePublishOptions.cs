using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("bot", "prepare-publish")]
public record AzBotPreparePublishOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--proj-file-path")] string ProjFilePath,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sln-name")] string SlnName
) : AzOptions
{
    [CliOption("--code-dir")]
    public string? CodeDir { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }
}