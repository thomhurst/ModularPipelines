using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3api", "list-objects")]
public record AwsS3apiListObjectsOptions(
[property: CliOption("--bucket")] string Bucket
) : AwsOptions
{
    [CliOption("--delimiter")]
    public string? Delimiter { get; set; }

    [CliOption("--encoding-type")]
    public string? EncodingType { get; set; }

    [CliOption("--prefix")]
    public string? Prefix { get; set; }

    [CliOption("--request-payer")]
    public string? RequestPayer { get; set; }

    [CliOption("--expected-bucket-owner")]
    public string? ExpectedBucketOwner { get; set; }

    [CliOption("--optional-object-attributes")]
    public string[]? OptionalObjectAttributes { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}