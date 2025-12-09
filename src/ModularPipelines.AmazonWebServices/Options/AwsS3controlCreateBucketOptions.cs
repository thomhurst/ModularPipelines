using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3control", "create-bucket")]
public record AwsS3controlCreateBucketOptions(
[property: CliOption("--bucket")] string Bucket
) : AwsOptions
{
    [CliOption("--acl")]
    public string? Acl { get; set; }

    [CliOption("--create-bucket-configuration")]
    public string? CreateBucketConfiguration { get; set; }

    [CliOption("--grant-full-control")]
    public string? GrantFullControl { get; set; }

    [CliOption("--grant-read")]
    public string? GrantRead { get; set; }

    [CliOption("--grant-read-acp")]
    public string? GrantReadAcp { get; set; }

    [CliOption("--grant-write")]
    public string? GrantWrite { get; set; }

    [CliOption("--grant-write-acp")]
    public string? GrantWriteAcp { get; set; }

    [CliOption("--outpost-id")]
    public string? OutpostId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}