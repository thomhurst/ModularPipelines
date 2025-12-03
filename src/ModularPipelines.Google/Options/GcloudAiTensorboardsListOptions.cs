using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "tensorboards", "list")]
public record GcloudAiTensorboardsListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}