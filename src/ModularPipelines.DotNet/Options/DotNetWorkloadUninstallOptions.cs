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

    [CliArgument(Name = "<WORKLOAD_ID...>")]
    public virtual string? WorkloadID { get; set; }
}
