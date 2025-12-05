using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "remove-tags-from-on-premises-instances")]
public record AwsDeployRemoveTagsFromOnPremisesInstancesOptions(
[property: CliOption("--tags")] string[] Tags,
[property: CliOption("--instance-names")] string[] InstanceNames
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}