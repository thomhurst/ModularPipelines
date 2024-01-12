using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("info")]
public record GcloudInfoOptions : GcloudOptions
{
    [BooleanCommandSwitch("--anonymize")]
    public bool? Anonymize { get; set; }

    [BooleanCommandSwitch("--run-diagnostics")]
    public bool? RunDiagnostics { get; set; }

    [BooleanCommandSwitch("--show-log")]
    public bool? ShowLog { get; set; }
}