using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("docdb", "delete-db-subnet-group")]
public record AwsDocdbDeleteDbSubnetGroupOptions(
[property: CommandSwitch("--db-subnet-group-name")] string DbSubnetGroupName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}