using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "update-repository-description")]
public record AwsCodecommitUpdateRepositoryDescriptionOptions(
[property: CliOption("--repository-name")] string RepositoryName
) : AwsOptions
{
    [CliOption("--repository-description")]
    public string? RepositoryDescription { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}