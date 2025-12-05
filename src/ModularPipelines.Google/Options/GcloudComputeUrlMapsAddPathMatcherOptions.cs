using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "url-maps", "add-path-matcher")]
public record GcloudComputeUrlMapsAddPathMatcherOptions(
[property: CliArgument] string UrlMap,
[property: CliOption("--path-matcher-name")] string PathMatcherName,
[property: CliOption("--default-backend-bucket")] string DefaultBackendBucket,
[property: CliOption("--default-service")] string DefaultService
) : GcloudOptions
{
    [CliOption("--backend-bucket-path-rules")]
    public string[]? BackendBucketPathRules { get; set; }

    [CliOption("--backend-service-path-rules")]
    public string[]? BackendServicePathRules { get; set; }

    [CliFlag("--delete-orphaned-path-matcher")]
    public bool? DeleteOrphanedPathMatcher { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--path-rules")]
    public string[]? PathRules { get; set; }

    [CliOption("--existing-host")]
    public string? ExistingHost { get; set; }

    [CliOption("--new-hosts")]
    public string[]? NewHosts { get; set; }

    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}