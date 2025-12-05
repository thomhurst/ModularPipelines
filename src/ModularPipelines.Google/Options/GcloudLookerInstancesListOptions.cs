using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("looker", "instances", "list")]
public record GcloudLookerInstancesListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}