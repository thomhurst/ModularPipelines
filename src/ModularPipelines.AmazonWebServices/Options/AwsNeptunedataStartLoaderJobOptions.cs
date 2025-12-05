using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptunedata", "start-loader-job")]
public record AwsNeptunedataStartLoaderJobOptions(
[property: CliOption("--source")] string Source,
[property: CliOption("--format")] string Format,
[property: CliOption("--s3-bucket-region")] string S3BucketRegion,
[property: CliOption("--iam-role-arn")] string IamRoleArn
) : AwsOptions
{
    [CliOption("--mode")]
    public string? Mode { get; set; }

    [CliOption("--parallelism")]
    public string? Parallelism { get; set; }

    [CliOption("--parser-configuration")]
    public IEnumerable<KeyValue>? ParserConfiguration { get; set; }

    [CliOption("--dependencies")]
    public string[]? Dependencies { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}