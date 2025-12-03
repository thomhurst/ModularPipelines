using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cdn", "endpoint", "create")]
public record AzCdnEndpointCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--origin")] string Origin,
[property: CliOption("--profile-name")] string ProfileName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--content-types-to-compress")]
    public string? ContentTypesToCompress { get; set; }

    [CliFlag("--enable-compression")]
    public bool? EnableCompression { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

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

    [CliOption("--query-string-caching")]
    public string? QueryStringCaching { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}