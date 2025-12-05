using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-context-manager", "perimeters", "list")]
public record GcloudAccessContextManagerPerimetersListOptions : GcloudOptions
{
    [CliOption("--policy")]
    public string? Policy { get; set; }
}