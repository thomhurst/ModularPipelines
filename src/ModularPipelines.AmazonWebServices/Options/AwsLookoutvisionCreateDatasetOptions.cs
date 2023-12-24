using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutvision", "create-dataset")]
public record AwsLookoutvisionCreateDatasetOptions(
[property: CommandSwitch("--project-name")] string ProjectName,
[property: CommandSwitch("--dataset-type")] string DatasetType
) : AwsOptions
{
    [CommandSwitch("--dataset-source")]
    public string? DatasetSource { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}