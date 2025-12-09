using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "update-repository-name")]
public record AwsCodecommitUpdateRepositoryNameOptions(
[property: CliOption("--old-name")] string OldName,
[property: CliOption("--new-name")] string NewName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}