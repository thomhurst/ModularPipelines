using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-groups", "group-resources")]
public record AwsResourceGroupsGroupResourcesOptions(
[property: CliOption("--group")] string Group,
[property: CliOption("--resource-arns")] string[] ResourceArns
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}