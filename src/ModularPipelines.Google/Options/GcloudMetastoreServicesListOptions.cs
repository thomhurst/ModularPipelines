using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("metastore", "services", "list")]
public record GcloudMetastoreServicesListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}