using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("athena", "update-notebook-metadata")]
public record AwsAthenaUpdateNotebookMetadataOptions(
[property: CommandSwitch("--notebook-id")] string NotebookId,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}