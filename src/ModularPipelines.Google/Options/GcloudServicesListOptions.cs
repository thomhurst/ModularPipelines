using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("services", "list")]
public record GcloudServicesListOptions : GcloudOptions
{
    [BooleanCommandSwitch("--available")]
    public bool? Available { get; set; }

    [BooleanCommandSwitch("--enabled")]
    public bool? Enabled { get; set; }
}