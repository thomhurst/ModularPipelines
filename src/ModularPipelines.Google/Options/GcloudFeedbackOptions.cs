using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("feedback")]
public record GcloudFeedbackOptions : GcloudOptions
{
    [CliOption("--log-file")]
    public string? LogFile { get; set; }
}