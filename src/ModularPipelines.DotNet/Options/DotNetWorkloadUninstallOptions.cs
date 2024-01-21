using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetWorkloadUninstallOptions : DotNetOptions
{
    [PositionalArgument(PlaceholderName = "<WORKLOAD_ID...>")]
    public string? WorkloadID { get; set; }
}
