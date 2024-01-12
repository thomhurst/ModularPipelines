using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "hmac", "list")]
public record GcloudStorageHmacListOptions : GcloudOptions
{
    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [BooleanCommandSwitch("--long")]
    public bool? Long { get; set; }

    [CommandSwitch("--service-account")]
    public string? ServiceAccount { get; set; }
}