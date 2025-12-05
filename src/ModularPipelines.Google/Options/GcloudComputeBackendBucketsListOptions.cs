using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "backend-buckets", "list")]
public record GcloudComputeBackendBucketsListOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--regexp")]
    public string? Regexp { get; set; }
}