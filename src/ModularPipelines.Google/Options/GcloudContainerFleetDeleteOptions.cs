using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "delete")]
public record GcloudContainerFleetDeleteOptions : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}