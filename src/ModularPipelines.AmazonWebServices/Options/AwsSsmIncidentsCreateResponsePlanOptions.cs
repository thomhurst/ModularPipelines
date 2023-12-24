using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm-incidents", "create-response-plan")]
public record AwsSsmIncidentsCreateResponsePlanOptions(
[property: CommandSwitch("--incident-template")] string IncidentTemplate,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--actions")]
    public string[]? Actions { get; set; }

    [CommandSwitch("--chat-channel")]
    public string? ChatChannel { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--engagements")]
    public string[]? Engagements { get; set; }

    [CommandSwitch("--integrations")]
    public string[]? Integrations { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}