using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workdocs", "describe-folder-contents")]
public record AwsWorkdocsDescribeFolderContentsOptions(
[property: CliOption("--folder-id")] string FolderId
) : AwsOptions
{
    [CliOption("--authentication-token")]
    public string? AuthenticationToken { get; set; }

    [CliOption("--sort")]
    public string? Sort { get; set; }

    [CliOption("--order")]
    public string? Order { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--include")]
    public string? Include { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}