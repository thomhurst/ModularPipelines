using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigee", "developers", "list")]
public record GcloudApigeeDevelopersListOptions : GcloudOptions
{
    [CommandSwitch("--organization")]
    public string? Organization { get; set; }
}