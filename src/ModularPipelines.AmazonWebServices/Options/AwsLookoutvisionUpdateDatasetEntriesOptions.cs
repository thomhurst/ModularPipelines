using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutvision", "update-dataset-entries")]
public record AwsLookoutvisionUpdateDatasetEntriesOptions(
[property: CommandSwitch("--project-name")] string ProjectName,
[property: CommandSwitch("--dataset-type")] string DatasetType,
[property: CommandSwitch("--changes")] string Changes
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}