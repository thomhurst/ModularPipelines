using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "list-aliases")]
public record AwsSagemakerListAliasesOptions(
[property: CliOption("--image-name")] string ImageName
) : AwsOptions
{
    [CliOption("--alias")]
    public string? Alias { get; set; }

    [CliOption("--version-number")]
    public int? VersionNumber { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}