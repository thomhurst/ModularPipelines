using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eks", "describe-addon")]
public record AwsEksDescribeAddonOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--addon-name")] string AddonName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}