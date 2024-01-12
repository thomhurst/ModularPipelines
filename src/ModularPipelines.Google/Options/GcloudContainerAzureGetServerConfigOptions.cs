using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "azure", "get-server-config")]
public record GcloudContainerAzureGetServerConfigOptions : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}