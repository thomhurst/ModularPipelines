using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bedrock-agent-runtime", "retrieve-and-generate")]
public record AwsBedrockAgentRuntimeRetrieveAndGenerateOptions(
[property: CommandSwitch("--input")] string Input
) : AwsOptions
{
    [CommandSwitch("--session-id")]
    public string? SessionId { get; set; }

    [CommandSwitch("--retrieve-and-generate-configuration")]
    public string? RetrieveAndGenerateConfiguration { get; set; }

    [CommandSwitch("--session-configuration")]
    public string? SessionConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}