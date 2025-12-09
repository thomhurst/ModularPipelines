using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wisdom", "delete-assistant-association")]
public record AwsWisdomDeleteAssistantAssociationOptions(
[property: CliOption("--assistant-association-id")] string AssistantAssociationId,
[property: CliOption("--assistant-id")] string AssistantId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}