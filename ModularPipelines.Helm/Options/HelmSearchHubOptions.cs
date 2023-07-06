using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("search", "hub")]
public record HelmSearchHubOptions : HelmOptions
{
    [CommandLongSwitch("endpoint", SwitchValueSeparator = " ")]
    public string? Endpoint { get; set; }

    [BooleanCommandSwitch("list-repo-url")]
    public bool? ListRepoUrl { get; set; }

    [CommandLongSwitch("max-col-width", SwitchValueSeparator = " ")]
    public string? MaxColWidth { get; set; }

    [CommandLongSwitch("output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

}
