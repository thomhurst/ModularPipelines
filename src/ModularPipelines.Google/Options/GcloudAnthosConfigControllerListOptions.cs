using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("anthos", "config", "controller", "list")]
public record GcloudAnthosConfigControllerListOptions : GcloudOptions
{
    [BooleanCommandSwitch("--full-name")]
    public bool? FullName { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }
}