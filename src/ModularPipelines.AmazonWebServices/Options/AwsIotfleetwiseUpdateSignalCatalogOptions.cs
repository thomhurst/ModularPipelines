using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotfleetwise", "update-signal-catalog")]
public record AwsIotfleetwiseUpdateSignalCatalogOptions(
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--nodes-to-add")]
    public string[]? NodesToAdd { get; set; }

    [CommandSwitch("--nodes-to-update")]
    public string[]? NodesToUpdate { get; set; }

    [CommandSwitch("--nodes-to-remove")]
    public string[]? NodesToRemove { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}