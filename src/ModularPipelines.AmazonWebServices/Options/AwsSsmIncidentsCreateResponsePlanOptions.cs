using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm-incidents", "create-response-plan")]
public record AwsSsmIncidentsCreateResponsePlanOptions(
[property: CliOption("--incident-template")] string IncidentTemplate,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--actions")]
    public string[]? Actions { get; set; }

    [CliOption("--chat-channel")]
    public string? ChatChannel { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--engagements")]
    public string[]? Engagements { get; set; }

    [CliOption("--integrations")]
    public string[]? Integrations { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}