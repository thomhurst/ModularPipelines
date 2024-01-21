using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetWorkloadUninstallOptions : DotNetOptions
{
    public DotNetWorkloadUninstallOptions(
        string workloadID
    )
    {
        CommandParts = ["workload", "uninstall", "<WORKLOAD_ID...>"];

        WorkloadID = workloadID;
    }

    [PositionalArgument(PlaceholderName = "<WORKLOAD_ID...>")]
    public string? WorkloadID { get; set; }
}
