using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("auth", "list")]
public record GcloudAuthListOptions : GcloudOptions
{
    [CommandSwitch("--filter-account")]
    public string? FilterAccount { get; set; }
}