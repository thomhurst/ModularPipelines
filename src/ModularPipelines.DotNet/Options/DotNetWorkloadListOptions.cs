using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CommandPrecedingArguments("workload", "list")]
[ExcludeFromCodeCoverage]
public record DotNetWorkloadListOptions : DotNetOptions
{
    [CommandSwitch("--verbosity")]
    public string? Verbosity { get; set; }
}
