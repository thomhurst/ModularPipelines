using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "bare-metal", "node-pools", "delete")]
public record GcloudContainerBareMetalNodePoolsDeleteOptions(
[property: PositionalArgument] string NodePool,
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--allow-missing")]
    public bool? AllowMissing { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [BooleanCommandSwitch("--ignore-errors")]
    public bool? IgnoreErrors { get; set; }

    [BooleanCommandSwitch("--validate-only")]
    public bool? ValidateOnly { get; set; }
}