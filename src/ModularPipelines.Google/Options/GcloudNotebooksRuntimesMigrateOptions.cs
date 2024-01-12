using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("notebooks", "runtimes", "migrate")]
public record GcloudNotebooksRuntimesMigrateOptions(
[property: PositionalArgument] string Runtime,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--post-startup-script-option")]
    public string? PostStartupScriptOption { get; set; }

    [CommandSwitch("--service-account")]
    public string? ServiceAccount { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }

    [CommandSwitch("--subnet-region")]
    public string? SubnetRegion { get; set; }
}