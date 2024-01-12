using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "azure", "clusters", "delete")]
public record GcloudContainerAzureClustersDeleteOptions(
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--allow-missing")]
    public bool? AllowMissing { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}