using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("info")]
public record GcloudInfoOptions : GcloudOptions
{
    [CliFlag("--anonymize")]
    public bool? Anonymize { get; set; }

    [CliFlag("--run-diagnostics")]
    public bool? RunDiagnostics { get; set; }

    [CliFlag("--show-log")]
    public bool? ShowLog { get; set; }
}