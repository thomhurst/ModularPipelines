using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "untag-resource")]
public record AwsBackupUntagResourceOptions(
[property: CliOption("--resource-arn")] string ResourceArn,
[property: CliOption("--tag-key-list")] string[] TagKeyList
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}