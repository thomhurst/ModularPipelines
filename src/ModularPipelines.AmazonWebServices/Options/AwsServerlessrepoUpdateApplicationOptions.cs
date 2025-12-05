using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("serverlessrepo", "update-application")]
public record AwsServerlessrepoUpdateApplicationOptions(
[property: CliOption("--application-id")] string ApplicationId
) : AwsOptions
{
    [CliOption("--author")]
    public string? Author { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--home-page-url")]
    public string? HomePageUrl { get; set; }

    [CliOption("--labels")]
    public string[]? Labels { get; set; }

    [CliOption("--readme-body")]
    public string? ReadmeBody { get; set; }

    [CliOption("--readme-url")]
    public string? ReadmeUrl { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}