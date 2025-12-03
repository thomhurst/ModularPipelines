using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("netapp", "kms-configs", "list")]
public record GcloudNetappKmsConfigsListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}