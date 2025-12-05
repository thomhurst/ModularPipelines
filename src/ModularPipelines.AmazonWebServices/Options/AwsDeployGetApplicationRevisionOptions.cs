using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "get-application-revision")]
public record AwsDeployGetApplicationRevisionOptions(
[property: CliOption("--application-name")] string ApplicationName
) : AwsOptions
{
    [CliOption("--revision")]
    public string? Revision { get; set; }

    [CliOption("--s3-location")]
    public string? S3Location { get; set; }

    [CliOption("--github-location")]
    public string? GithubLocation { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}