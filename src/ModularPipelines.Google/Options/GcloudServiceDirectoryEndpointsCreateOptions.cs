using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("service-directory", "endpoints", "create")]
public record GcloudServiceDirectoryEndpointsCreateOptions(
[property: CliArgument] string Endpoint,
[property: CliArgument] string Location,
[property: CliArgument] string Namespace,
[property: CliArgument] string Service
) : GcloudOptions
{
    [CliOption("--address")]
    public string? Address { get; set; }

    [CliOption("--annotations")]
    public IEnumerable<KeyValue>? Annotations { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--port")]
    public string? Port { get; set; }
}