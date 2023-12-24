using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dax", "create-subnet-group")]
public record AwsDaxCreateSubnetGroupOptions(
[property: CommandSwitch("--subnet-group-name")] string SubnetGroupName,
[property: CommandSwitch("--subnet-ids")] string[] SubnetIds
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}