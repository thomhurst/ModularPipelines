using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apim", "api", "release", "create")]
public record AzApimApiReleaseCreateOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--api-revision")] string ApiRevision,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-name")] string ServiceName
) : AzOptions
{
    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--notes")]
    public string? Notes { get; set; }

    [CliOption("--release-id")]
    public string? ReleaseId { get; set; }
}