using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "attached", "operations", "list")]
public record GcloudContainerAttachedOperationsListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}