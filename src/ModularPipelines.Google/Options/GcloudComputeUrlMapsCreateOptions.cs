using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "url-maps", "create")]
public record GcloudComputeUrlMapsCreateOptions(
[property: CliArgument] string UrlMap,
[property: CliOption("--default-backend-bucket")] string DefaultBackendBucket,
[property: CliOption("--default-service")] string DefaultService
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}