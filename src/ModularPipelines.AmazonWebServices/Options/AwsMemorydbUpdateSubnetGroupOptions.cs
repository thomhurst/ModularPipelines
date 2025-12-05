using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("memorydb", "update-subnet-group")]
public record AwsMemorydbUpdateSubnetGroupOptions(
[property: CliOption("--subnet-group-name")] string SubnetGroupName
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--subnet-ids")]
    public string[]? SubnetIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}