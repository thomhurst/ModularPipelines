using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "aws", "get-server-config")]
public record GcloudContainerAwsGetServerConfigOptions : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}