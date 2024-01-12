using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "agents", "delete")]
public record GcloudTransferAgentsDeleteOptions : GcloudOptions
{
    [CommandSwitch("--ids")]
    public string[]? Ids { get; set; }

    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [BooleanCommandSwitch("--uninstall")]
    public bool? Uninstall { get; set; }
}