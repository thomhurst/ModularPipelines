using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("service-directory", "endpoints", "create")]
public record GcloudServiceDirectoryEndpointsCreateOptions(
[property: PositionalArgument] string Endpoint,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Namespace,
[property: PositionalArgument] string Service
) : GcloudOptions
{
    [CommandSwitch("--address")]
    public string? Address { get; set; }

    [CommandSwitch("--annotations")]
    public IEnumerable<KeyValue>? Annotations { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--port")]
    public string? Port { get; set; }
}