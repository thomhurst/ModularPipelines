using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scc", "custom-modules", "sha", "update")]
public record GcloudSccCustomModulesShaUpdateOptions(
[property: PositionalArgument] string CustomModule
) : GcloudOptions
{
    [CommandSwitch("--custom-config-from-file")]
    public string? CustomConfigFromFile { get; set; }

    [CommandSwitch("--enablement-state")]
    public string? EnablementState { get; set; }

    [CommandSwitch("--update-mask")]
    public string? UpdateMask { get; set; }

    [CommandSwitch("--folder")]
    public string? Folder { get; set; }

    [CommandSwitch("--organization")]
    public string? Organization { get; set; }

    [CommandSwitch("--project")]
    public new string? Project { get; set; }
}