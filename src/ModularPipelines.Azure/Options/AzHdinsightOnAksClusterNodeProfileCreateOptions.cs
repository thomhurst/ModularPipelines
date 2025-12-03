using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("hdinsight-on-aks", "cluster", "node-profile", "create")]
public record AzHdinsightOnAksClusterNodeProfileCreateOptions(
[property: CliOption("--count")] int Count,
[property: CliOption("--node-type")] string NodeType,
[property: CliOption("--vm-size")] string VmSize
) : AzOptions;