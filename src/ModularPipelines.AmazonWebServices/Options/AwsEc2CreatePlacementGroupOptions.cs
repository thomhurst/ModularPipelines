using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-placement-group")]
public record AwsEc2CreatePlacementGroupOptions : AwsOptions
{
    [CliOption("--group-name")]
    public string? GroupName { get; set; }

    [CliOption("--strategy")]
    public string? Strategy { get; set; }

    [CliOption("--partition-count")]
    public int? PartitionCount { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--spread-level")]
    public string? SpreadLevel { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}