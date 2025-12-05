using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("proton", "create-service")]
public record AwsProtonCreateServiceOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--spec")] string Spec,
[property: CliOption("--template-major-version")] string TemplateMajorVersion,
[property: CliOption("--template-name")] string TemplateName
) : AwsOptions
{
    [CliOption("--branch-name")]
    public string? BranchName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--repository-connection-arn")]
    public string? RepositoryConnectionArn { get; set; }

    [CliOption("--repository-id")]
    public string? RepositoryId { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--template-minor-version")]
    public string? TemplateMinorVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}