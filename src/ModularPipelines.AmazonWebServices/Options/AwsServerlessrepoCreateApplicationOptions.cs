using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("serverlessrepo", "create-application")]
public record AwsServerlessrepoCreateApplicationOptions(
[property: CliOption("--author")] string Author,
[property: CliOption("--description")] string Description,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--home-page-url")]
    public string? HomePageUrl { get; set; }

    [CliOption("--labels")]
    public string[]? Labels { get; set; }

    [CliOption("--license-body")]
    public string? LicenseBody { get; set; }

    [CliOption("--license-url")]
    public string? LicenseUrl { get; set; }

    [CliOption("--readme-body")]
    public string? ReadmeBody { get; set; }

    [CliOption("--readme-url")]
    public string? ReadmeUrl { get; set; }

    [CliOption("--semantic-version")]
    public string? SemanticVersion { get; set; }

    [CliOption("--source-code-archive-url")]
    public string? SourceCodeArchiveUrl { get; set; }

    [CliOption("--source-code-url")]
    public string? SourceCodeUrl { get; set; }

    [CliOption("--spdx-license-id")]
    public string? SpdxLicenseId { get; set; }

    [CliOption("--template-body")]
    public string? TemplateBody { get; set; }

    [CliOption("--template-url")]
    public string? TemplateUrl { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}