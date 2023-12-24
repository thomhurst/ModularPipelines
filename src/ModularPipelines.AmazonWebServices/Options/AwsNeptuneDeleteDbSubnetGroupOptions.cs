using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("neptune", "delete-db-subnet-group")]
public record AwsNeptuneDeleteDbSubnetGroupOptions(
[property: CommandSwitch("--db-subnet-group-name")] string DbSubnetGroupName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}