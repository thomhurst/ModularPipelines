using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigee", "environments", "list")]
public record GcloudApigeeEnvironmentsListOptions : GcloudOptions
{
    [CliOption("--organization")]
    public string? Organization { get; set; }
}