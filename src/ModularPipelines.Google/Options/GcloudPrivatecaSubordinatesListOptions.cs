using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("privateca", "subordinates", "list")]
public record GcloudPrivatecaSubordinatesListOptions : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--pool")]
    public string? Pool { get; set; }
}