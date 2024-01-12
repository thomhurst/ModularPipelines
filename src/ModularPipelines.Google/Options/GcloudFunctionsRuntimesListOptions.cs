using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functions", "runtimes", "list")]
public record GcloudFunctionsRuntimesListOptions : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}