using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "associate-default-vocabulary")]
public record AwsConnectAssociateDefaultVocabularyOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--language-code")] string LanguageCode
) : AwsOptions
{
    [CommandSwitch("--vocabulary-id")]
    public string? VocabularyId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}