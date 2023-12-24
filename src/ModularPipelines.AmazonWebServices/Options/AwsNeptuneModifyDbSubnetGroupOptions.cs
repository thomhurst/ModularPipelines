using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("neptune", "modify-db-subnet-group")]
public record AwsNeptuneModifyDbSubnetGroupOptions(
[property: CommandSwitch("--db-subnet-group-name")] string DbSubnetGroupName,
[property: CommandSwitch("--subnet-ids")] string[] SubnetIds
) : AwsOptions
{
    [CommandSwitch("--db-subnet-group-description")]
    public string? DbSubnetGroupDescription { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}