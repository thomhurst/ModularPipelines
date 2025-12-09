using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eks", "describe-update")]
public record AwsEksDescribeUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--update-id")] string UpdateId
) : AwsOptions
{
    [CliOption("--nodegroup-name")]
    public string? NodegroupName { get; set; }

    [CliOption("--addon-name")]
    public string? AddonName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}