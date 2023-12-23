using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotfleetwise", "create-model-manifest")]
public record AwsIotfleetwiseCreateModelManifestOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--nodes")] string[] Nodes,
[property: CommandSwitch("--signal-catalog-arn")] string SignalCatalogArn
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}