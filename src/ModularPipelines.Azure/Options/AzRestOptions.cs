using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rest")]
public record AzRestOptions(
[property: CommandSwitch("--uri")] string Uri
) : AzOptions
{
    [CommandSwitch("--body")]
    public string? Body { get; set; }

    [CommandSwitch("--headers")]
    public string? Headers { get; set; }

    [CommandSwitch("--method")]
    public string? Method { get; set; }

    [CommandSwitch("--output-file")]
    public string? OutputFile { get; set; }

    [CommandSwitch("--resource")]
    public string? Resource { get; set; }

    [BooleanCommandSwitch("--skip-authorization-header")]
    public bool? SkipAuthorizationHeader { get; set; }

    [CommandSwitch("--uri-parameters")]
    public string? UriParameters { get; set; }
}