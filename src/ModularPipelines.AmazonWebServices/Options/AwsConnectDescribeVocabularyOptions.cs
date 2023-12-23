using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "describe-vocabulary")]
public record AwsConnectDescribeVocabularyOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--vocabulary-id")] string VocabularyId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}