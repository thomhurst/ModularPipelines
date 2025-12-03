using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "indexes", "list")]
public record GcloudAiIndexesListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}