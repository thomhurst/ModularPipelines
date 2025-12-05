using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-context-manager", "levels", "list")]
public record GcloudAccessContextManagerLevelsListOptions : GcloudOptions
{
    [CliOption("--policy")]
    public string? Policy { get; set; }
}