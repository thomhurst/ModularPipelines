using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CliCommand("workload")]
[ExcludeFromCodeCoverage]
public record DotNetWorkloadOptions : DotNetOptions
{
    [CliFlag("--info")]
    public virtual bool? Info { get; set; }
}
