using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("builds", "triggers", "list")]
public record GcloudBuildsTriggersListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}