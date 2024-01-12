using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "get-config")]
public record GcloudDeployGetConfigOptions : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}