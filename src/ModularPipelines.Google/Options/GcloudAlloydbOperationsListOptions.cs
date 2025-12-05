using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("alloydb", "operations", "list")]
public record GcloudAlloydbOperationsListOptions : GcloudOptions
{
    [CliOption("--cluster")]
    public string? Cluster { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}