using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("run", "regions", "list")]
public record GcloudRunRegionsListOptions : GcloudOptions
{
    [CommandSwitch("--platform")]
    public string? Platform { get; set; }
}