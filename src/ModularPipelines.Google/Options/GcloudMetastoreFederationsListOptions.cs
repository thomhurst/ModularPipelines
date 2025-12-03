using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("metastore", "federations", "list")]
public record GcloudMetastoreFederationsListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}