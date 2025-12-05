using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "attached", "get-server-config")]
public record GcloudContainerAttachedGetServerConfigOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}