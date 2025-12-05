using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigee", "apis", "list")]
public record GcloudApigeeApisListOptions : GcloudOptions
{
    [CliOption("--organization")]
    public string? Organization { get; set; }
}