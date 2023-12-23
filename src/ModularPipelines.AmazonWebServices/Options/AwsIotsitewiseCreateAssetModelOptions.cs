using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotsitewise", "create-asset-model")]
public record AwsIotsitewiseCreateAssetModelOptions(
[property: CommandSwitch("--asset-model-name")] string AssetModelName
) : AwsOptions
{
    [CommandSwitch("--asset-model-description")]
    public string? AssetModelDescription { get; set; }

    [CommandSwitch("--asset-model-properties")]
    public string[]? AssetModelProperties { get; set; }

    [CommandSwitch("--asset-model-hierarchies")]
    public string[]? AssetModelHierarchies { get; set; }

    [CommandSwitch("--asset-model-composite-models")]
    public string[]? AssetModelCompositeModels { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--asset-model-id")]
    public string? AssetModelId { get; set; }

    [CommandSwitch("--asset-model-external-id")]
    public string? AssetModelExternalId { get; set; }

    [CommandSwitch("--asset-model-type")]
    public string? AssetModelType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}