using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "hmac", "list")]
public record GcloudStorageHmacListOptions : GcloudOptions
{
    [CliFlag("--all")]
    public bool? All { get; set; }

    [CliFlag("--long")]
    public bool? Long { get; set; }

    [CliOption("--service-account")]
    public string? ServiceAccount { get; set; }
}