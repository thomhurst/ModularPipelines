using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigee", "apis", "list")]
public record GcloudApigeeApisListOptions : GcloudOptions
{
    [CommandSwitch("--organization")]
    public string? Organization { get; set; }
}