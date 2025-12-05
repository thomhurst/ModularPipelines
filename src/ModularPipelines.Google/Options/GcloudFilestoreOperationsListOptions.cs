using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("filestore", "operations", "list")]
public record GcloudFilestoreOperationsListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}