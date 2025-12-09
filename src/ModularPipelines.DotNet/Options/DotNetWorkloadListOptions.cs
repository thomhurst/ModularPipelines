using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CliSubCommand("workload", "list")]
[ExcludeFromCodeCoverage]
public record DotNetWorkloadListOptions : DotNetOptions
{
    [CliOption("--verbosity")]
    public virtual string? Verbosity { get; set; }
}
