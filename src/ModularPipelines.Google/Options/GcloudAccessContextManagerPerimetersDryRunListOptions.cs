using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-context-manager", "perimeters", "dry-run", "list")]
public record GcloudAccessContextManagerPerimetersDryRunListOptions : GcloudOptions
{
    [CliOption("--policy")]
    public string? Policy { get; set; }
}