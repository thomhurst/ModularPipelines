using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-context-manager", "cloud-bindings", "list")]
public record GcloudAccessContextManagerCloudBindingsListOptions : GcloudOptions
{
    [CommandSwitch("--organization")]
    public string? Organization { get; set; }
}