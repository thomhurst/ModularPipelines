using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("looker", "operations", "list")]
public record GcloudLookerOperationsListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}