using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-context-manager", "perimeters", "dry-run", "enforce-all")]
public record GcloudAccessContextManagerPerimetersDryRunEnforceAllOptions : GcloudOptions
{
    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliOption("--policy")]
    public string? Policy { get; set; }
}