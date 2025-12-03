using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bms", "instances", "list")]
public record GcloudBmsInstancesListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}