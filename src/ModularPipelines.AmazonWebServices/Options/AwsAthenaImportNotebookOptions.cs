using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("athena", "import-notebook")]
public record AwsAthenaImportNotebookOptions(
[property: CommandSwitch("--work-group")] string WorkGroup,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--payload")] string Payload,
[property: CommandSwitch("--type")] string Type
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}