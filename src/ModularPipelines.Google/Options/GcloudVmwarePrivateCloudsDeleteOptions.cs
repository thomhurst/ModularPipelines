using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "private-clouds", "delete")]
public record GcloudVmwarePrivateCloudsDeleteOptions(
[property: PositionalArgument] string PrivateCloud,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--delay-hours")]
    public string? DelayHours { get; set; }
}