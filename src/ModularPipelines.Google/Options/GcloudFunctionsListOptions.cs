using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functions", "list")]
public record GcloudFunctionsListOptions : GcloudOptions
{
    [CommandSwitch("--regions")]
    public string[]? Regions { get; set; }
}