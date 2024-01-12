using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-manager", "folders", "create")]
public record GcloudResourceManagerFoldersCreateOptions(
[property: CommandSwitch("--display-name")] string DisplayName
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--folder")]
    public string? Folder { get; set; }

    [CommandSwitch("--organization")]
    public string? Organization { get; set; }
}