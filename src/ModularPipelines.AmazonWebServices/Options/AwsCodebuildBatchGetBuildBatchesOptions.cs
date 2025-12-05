using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codebuild", "batch-get-build-batches")]
public record AwsCodebuildBatchGetBuildBatchesOptions(
[property: CliOption("--ids")] string[] Ids
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}