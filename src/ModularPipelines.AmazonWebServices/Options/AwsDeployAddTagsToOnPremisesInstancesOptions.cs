using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "add-tags-to-on-premises-instances")]
public record AwsDeployAddTagsToOnPremisesInstancesOptions(
[property: CliOption("--tags")] string[] Tags,
[property: CliOption("--instance-names")] string[] InstanceNames
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}