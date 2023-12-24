using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("fms", "batch-disassociate-resource")]
public record AwsFmsBatchDisassociateResourceOptions(
[property: CommandSwitch("--resource-set-identifier")] string ResourceSetIdentifier,
[property: CommandSwitch("--items")] string[] Items
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}