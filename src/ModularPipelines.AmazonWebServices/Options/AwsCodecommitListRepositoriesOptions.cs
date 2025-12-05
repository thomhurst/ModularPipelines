using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "list-repositories")]
public record AwsCodecommitListRepositoriesOptions : AwsOptions
{
    [CliOption("--sort-by")]
    public string? SortBy { get; set; }

    [CliOption("--order")]
    public string? Order { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}