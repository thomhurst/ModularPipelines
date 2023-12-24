using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("neptunedata", "start-loader-job")]
public record AwsNeptunedataStartLoaderJobOptions(
[property: CommandSwitch("--source")] string Source,
[property: CommandSwitch("--format")] string Format,
[property: CommandSwitch("--s3-bucket-region")] string S3BucketRegion,
[property: CommandSwitch("--iam-role-arn")] string IamRoleArn
) : AwsOptions
{
    [CommandSwitch("--mode")]
    public string? Mode { get; set; }

    [CommandSwitch("--parallelism")]
    public string? Parallelism { get; set; }

    [CommandSwitch("--parser-configuration")]
    public IEnumerable<KeyValue>? ParserConfiguration { get; set; }

    [CommandSwitch("--dependencies")]
    public string[]? Dependencies { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}