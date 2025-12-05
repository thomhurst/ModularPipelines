using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "get-differences")]
public record AwsCodecommitGetDifferencesOptions(
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--after-commit-specifier")] string AfterCommitSpecifier
) : AwsOptions
{
    [CliOption("--before-commit-specifier")]
    public string? BeforeCommitSpecifier { get; set; }

    [CliOption("--before-path")]
    public string? BeforePath { get; set; }

    [CliOption("--after-path")]
    public string? AfterPath { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}