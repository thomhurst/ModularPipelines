using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "update")]
public record GcloudAppUpdateOptions : GcloudOptions
{
    [CliOption("--service-account")]
    public string? ServiceAccount { get; set; }

    [CliOption("--[no-]split-health-checks")]
    public string[]? NoSplitHealthChecks { get; set; }
}