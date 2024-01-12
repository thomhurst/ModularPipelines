using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scc", "custom-modules", "sha", "create")]
public record GcloudSccCustomModulesShaCreateOptions(
[property: CommandSwitch("--custom-config-from-file")] string CustomConfigFromFile,
[property: CommandSwitch("--display-name")] string DisplayName,
[property: CommandSwitch("--enablement-state")] string EnablementState
) : GcloudOptions
{
    [CommandSwitch("--folder")]
    public string? Folder { get; set; }

    [CommandSwitch("--organization")]
    public string? Organization { get; set; }

    [CommandSwitch("--project")]
    public new string? Project { get; set; }
}