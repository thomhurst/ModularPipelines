using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataproc", "clusters", "list")]
public record GcloudDataprocClustersListOptions : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}