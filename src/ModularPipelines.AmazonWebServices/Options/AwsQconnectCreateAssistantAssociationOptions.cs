using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qconnect", "create-assistant-association")]
public record AwsQconnectCreateAssistantAssociationOptions(
[property: CommandSwitch("--assistant-id")] string AssistantId,
[property: CommandSwitch("--association")] string Association,
[property: CommandSwitch("--association-type")] string AssociationType
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}