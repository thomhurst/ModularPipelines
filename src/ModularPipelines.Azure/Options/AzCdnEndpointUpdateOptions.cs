using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cdn", "endpoint", "update")]
public record AzCdnEndpointUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--content-types-to-compress")]
    public string? ContentTypesToCompress { get; set; }

    [CliOption("--default-origin-group")]
    public string? DefaultOriginGroup { get; set; }

    [CliFlag("--enable-compression")]
    public bool? EnableCompression { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-http")]
    public bool? NoHttp { get; set; }

    [CliFlag("--no-https")]
    public bool? NoHttps { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--origin-host-header")]
    public string? OriginHostHeader { get; set; }

    [CliOption("--origin-path")]
    public string? OriginPath { get; set; }

    [CliOption("--profile-name")]
    public string? ProfileName { get; set; }

    [CliOption("--query-string-caching")]
    public string? QueryStringCaching { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}