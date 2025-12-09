using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "list-tags-for-resource")]
public record AwsDmsListTagsForResourceOptions : AwsOptions
{
    [CliOption("--resource-arn")]
    public string? ResourceArn { get; set; }

    [CliOption("--resource-arn-list")]
    public string[]? ResourceArnList { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}