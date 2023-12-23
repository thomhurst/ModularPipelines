using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3control", "create-bucket")]
public record AwsS3controlCreateBucketOptions(
[property: CommandSwitch("--bucket")] string Bucket
) : AwsOptions
{
    [CommandSwitch("--acl")]
    public string? Acl { get; set; }

    [CommandSwitch("--create-bucket-configuration")]
    public string? CreateBucketConfiguration { get; set; }

    [CommandSwitch("--grant-full-control")]
    public string? GrantFullControl { get; set; }

    [CommandSwitch("--grant-read")]
    public string? GrantRead { get; set; }

    [CommandSwitch("--grant-read-acp")]
    public string? GrantReadAcp { get; set; }

    [CommandSwitch("--grant-write")]
    public string? GrantWrite { get; set; }

    [CommandSwitch("--grant-write-acp")]
    public string? GrantWriteAcp { get; set; }

    [CommandSwitch("--outpost-id")]
    public string? OutpostId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}