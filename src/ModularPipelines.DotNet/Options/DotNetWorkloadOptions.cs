using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CommandPrecedingArguments("workload")]
[ExcludeFromCodeCoverage]
public record DotNetWorkloadOptions : DotNetOptions
{
    [BooleanCommandSwitch("--info")]
    public bool? Info { get; set; }
}
