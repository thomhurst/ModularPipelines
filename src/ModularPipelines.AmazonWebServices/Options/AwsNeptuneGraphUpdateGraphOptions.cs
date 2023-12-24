using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("neptune-graph", "update-graph")]
public record AwsNeptuneGraphUpdateGraphOptions(
[property: CommandSwitch("--graph-identifier")] string GraphIdentifier
) : AwsOptions
{
    [CommandSwitch("--provisioned-memory")]
    public int? ProvisionedMemory { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}