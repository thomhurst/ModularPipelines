using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appflow", "create-flow")]
public record AwsAppflowCreateFlowOptions(
[property: CommandSwitch("--flow-name")] string FlowName,
[property: CommandSwitch("--trigger-config")] string TriggerConfig,
[property: CommandSwitch("--source-flow-config")] string SourceFlowConfig,
[property: CommandSwitch("--destination-flow-config-list")] string[] DestinationFlowConfigList,
[property: CommandSwitch("--tasks")] string[] Tasks
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--kms-arn")]
    public string? KmsArn { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--metadata-catalog-config")]
    public string? MetadataCatalogConfig { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}