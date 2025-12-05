using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("components", "update")]
public record GcloudComponentsUpdateOptions : GcloudOptions
{
    [CliOption("--version")]
    public new string? Version { get; set; }
}