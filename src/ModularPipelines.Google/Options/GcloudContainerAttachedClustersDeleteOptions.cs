using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "attached", "clusters", "delete")]
public record GcloudContainerAttachedClustersDeleteOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--allow-missing")]
    public bool? AllowMissing { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--ignore-errors")]
    public bool? IgnoreErrors { get; set; }

    [CliFlag("--validate-only")]
    public bool? ValidateOnly { get; set; }
}