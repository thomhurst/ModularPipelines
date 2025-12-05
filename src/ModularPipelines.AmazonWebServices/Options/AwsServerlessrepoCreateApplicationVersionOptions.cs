using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("serverlessrepo", "create-application-version")]
public record AwsServerlessrepoCreateApplicationVersionOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--semantic-version")] string SemanticVersion
) : AwsOptions
{
    [CliOption("--source-code-archive-url")]
    public string? SourceCodeArchiveUrl { get; set; }

    [CliOption("--source-code-url")]
    public string? SourceCodeUrl { get; set; }

    [CliOption("--template-body")]
    public string? TemplateBody { get; set; }

    [CliOption("--template-url")]
    public string? TemplateUrl { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}