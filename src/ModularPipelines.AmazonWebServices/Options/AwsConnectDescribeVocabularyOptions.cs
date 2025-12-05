using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "describe-vocabulary")]
public record AwsConnectDescribeVocabularyOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--vocabulary-id")] string VocabularyId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}