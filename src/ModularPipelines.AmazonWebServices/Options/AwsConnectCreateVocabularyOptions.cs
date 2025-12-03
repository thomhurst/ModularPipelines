using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "create-vocabulary")]
public record AwsConnectCreateVocabularyOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--vocabulary-name")] string VocabularyName,
[property: CliOption("--language-code")] string LanguageCode,
[property: CliOption("--content")] string Content
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}