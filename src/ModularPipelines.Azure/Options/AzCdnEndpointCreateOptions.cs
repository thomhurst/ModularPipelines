using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cdn", "endpoint", "create")]
public record AzCdnEndpointCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--origin")] string Origin,
[property: CommandSwitch("--profile-name")] string ProfileName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--content-types-to-compress")]
    public string? ContentTypesToCompress { get; set; }

    [BooleanCommandSwitch("--enable-compression")]
    public bool? EnableCompression { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

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

    [CommandSwitch("--query-string-caching")]
    public string? QueryStringCaching { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}