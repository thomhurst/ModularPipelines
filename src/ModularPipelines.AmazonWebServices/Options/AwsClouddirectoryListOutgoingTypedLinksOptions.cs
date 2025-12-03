using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("clouddirectory", "list-outgoing-typed-links")]
public record AwsClouddirectoryListOutgoingTypedLinksOptions(
[property: CliOption("--directory-arn")] string DirectoryArn,
[property: CliOption("--object-reference")] string ObjectReference
) : AwsOptions
{
    [CliOption("--filter-attribute-ranges")]
    public string[]? FilterAttributeRanges { get; set; }

    [CliOption("--filter-typed-link")]
    public string? FilterTypedLink { get; set; }

    [CliOption("--consistency-level")]
    public string? ConsistencyLevel { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}