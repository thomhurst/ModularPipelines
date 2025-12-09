using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("clouddirectory", "batch-write")]
public record AwsClouddirectoryBatchWriteOptions(
[property: CliOption("--directory-arn")] string DirectoryArn,
[property: CliOption("--operations")] string[] Operations
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}