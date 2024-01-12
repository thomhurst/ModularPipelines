using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "apply")]
public record GcloudDeployApplyOptions(
[property: CommandSwitch("--file")] string File
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}