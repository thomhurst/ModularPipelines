using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("athena", "update-notebook")]
public record AwsAthenaUpdateNotebookOptions(
[property: CommandSwitch("--notebook-id")] string NotebookId,
[property: CommandSwitch("--payload")] string Payload,
[property: CommandSwitch("--type")] string Type
) : AwsOptions
{
    [CommandSwitch("--session-id")]
    public string? SessionId { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}