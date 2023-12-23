using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "create-vocabulary")]
public record AwsConnectCreateVocabularyOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--vocabulary-name")] string VocabularyName,
[property: CommandSwitch("--language-code")] string LanguageCode,
[property: CommandSwitch("--content")] string Content
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}