using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appflow", "update-flow")]
public record AwsAppflowUpdateFlowOptions(
[property: CliOption("--flow-name")] string FlowName,
[property: CliOption("--trigger-config")] string TriggerConfig,
[property: CliOption("--source-flow-config")] string SourceFlowConfig,
[property: CliOption("--destination-flow-config-list")] string[] DestinationFlowConfigList,
[property: CliOption("--tasks")] string[] Tasks
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--metadata-catalog-config")]
    public string? MetadataCatalogConfig { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}