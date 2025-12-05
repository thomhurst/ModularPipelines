using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigee", "developers", "list")]
public record GcloudApigeeDevelopersListOptions : GcloudOptions
{
    [CliOption("--organization")]
    public string? Organization { get; set; }
}