using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "azure", "clusters", "delete")]
public record GcloudContainerAzureClustersDeleteOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--allow-missing")]
    public bool? AllowMissing { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }
}