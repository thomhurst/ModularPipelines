using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloud", "list-profiles")]
public record AzCloudListProfilesOptions : AzOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--show-all")]
    public bool? ShowAll { get; set; }
}