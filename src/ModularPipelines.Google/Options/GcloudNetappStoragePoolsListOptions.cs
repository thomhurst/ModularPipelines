using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("netapp", "storage-pools", "list")]
public record GcloudNetappStoragePoolsListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}