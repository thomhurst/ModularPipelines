using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("feedback")]
public record GcloudFeedbackOptions : GcloudOptions
{
    [CommandSwitch("--log-file")]
    public string? LogFile { get; set; }
}