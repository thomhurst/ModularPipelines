using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "get-segments")]
public record AwsPinpointGetSegmentsOptions(
[property: CliOption("--application-id")] string ApplicationId
) : AwsOptions
{
    [CliOption("--page-size")]
    public string? PageSize { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}