using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cdn", "endpoint", "update")]
public record AzCdnEndpointUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--content-types-to-compress")]
    public string? ContentTypesToCompress { get; set; }

    [CommandSwitch("--default-origin-group")]
    public string? DefaultOriginGroup { get; set; }

    [BooleanCommandSwitch("--enable-compression")]
    public bool? EnableCompression { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-http")]
    public bool? NoHttp { get; set; }

    [BooleanCommandSwitch("--no-https")]
    public bool? NoHttps { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--origin-host-header")]
    public string? OriginHostHeader { get; set; }

    [CommandSwitch("--origin-path")]
    public string? OriginPath { get; set; }

    [CommandSwitch("--profile-name")]
    public string? ProfileName { get; set; }

    [CommandSwitch("--query-string-caching")]
    public string? QueryStringCaching { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}