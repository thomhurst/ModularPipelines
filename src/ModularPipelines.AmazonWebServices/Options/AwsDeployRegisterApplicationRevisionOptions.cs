using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "register-application-revision")]
public record AwsDeployRegisterApplicationRevisionOptions(
[property: CliOption("--application-name")] string ApplicationName
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--revision")]
    public string? Revision { get; set; }

    [CliOption("--s3-location")]
    public string? S3Location { get; set; }

    [CliOption("--github-location")]
    public string? GithubLocation { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}