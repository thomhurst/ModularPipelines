using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigee", "environments", "list")]
public record GcloudApigeeEnvironmentsListOptions : GcloudOptions
{
    [CommandSwitch("--organization")]
    public string? Organization { get; set; }
}