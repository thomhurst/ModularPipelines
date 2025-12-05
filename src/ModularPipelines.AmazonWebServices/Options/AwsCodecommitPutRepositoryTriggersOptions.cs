using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "put-repository-triggers")]
public record AwsCodecommitPutRepositoryTriggersOptions(
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--triggers")] string[] Triggers
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}