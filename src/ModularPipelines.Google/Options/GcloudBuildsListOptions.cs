using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("builds", "list")]
public record GcloudBuildsListOptions : GcloudOptions
{
    [BooleanCommandSwitch("--ongoing")]
    public bool? Ongoing { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}