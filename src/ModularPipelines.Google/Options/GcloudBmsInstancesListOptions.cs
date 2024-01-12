using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bms", "instances", "list")]
public record GcloudBmsInstancesListOptions : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}