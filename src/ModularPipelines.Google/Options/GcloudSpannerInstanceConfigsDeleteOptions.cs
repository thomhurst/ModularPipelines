using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spanner", "instance-configs", "delete")]
public record GcloudSpannerInstanceConfigsDeleteOptions(
[property: PositionalArgument] string InstanceConfig
) : GcloudOptions
{
    [CommandSwitch("--etag")]
    public string? Etag { get; set; }

    [BooleanCommandSwitch("--validate-only")]
    public bool? ValidateOnly { get; set; }
}