using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bms", "nfs-shares", "list")]
public record GcloudBmsNfsSharesListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}