using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("docdb", "create-db-subnet-group")]
public record AwsDocdbCreateDbSubnetGroupOptions(
[property: CommandSwitch("--db-subnet-group-name")] string DbSubnetGroupName,
[property: CommandSwitch("--db-subnet-group-description")] string DbSubnetGroupDescription,
[property: CommandSwitch("--subnet-ids")] string[] SubnetIds
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}