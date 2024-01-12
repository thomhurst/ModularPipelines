using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("privateca", "templates", "list")]
public record GcloudPrivatecaTemplatesListOptions : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}