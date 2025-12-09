using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bedrock-agent-runtime", "retrieve-and-generate")]
public record AwsBedrockAgentRuntimeRetrieveAndGenerateOptions(
[property: CliOption("--input")] string Input
) : AwsOptions
{
    [CliOption("--session-id")]
    public string? SessionId { get; set; }

    [CliOption("--retrieve-and-generate-configuration")]
    public string? RetrieveAndGenerateConfiguration { get; set; }

    [CliOption("--session-configuration")]
    public string? SessionConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}