using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("clouddirectory", "list-object-parents")]
public record AwsClouddirectoryListObjectParentsOptions(
[property: CliOption("--directory-arn")] string DirectoryArn,
[property: CliOption("--object-reference")] string ObjectReference
) : AwsOptions
{
    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--consistency-level")]
    public string? ConsistencyLevel { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}