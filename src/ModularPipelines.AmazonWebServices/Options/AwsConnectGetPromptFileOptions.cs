using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "get-prompt-file")]
public record AwsConnectGetPromptFileOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--prompt-id")] string PromptId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}