using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-groups", "ungroup-resources")]
public record AwsResourceGroupsUngroupResourcesOptions(
[property: CliOption("--group")] string Group,
[property: CliOption("--resource-arns")] string[] ResourceArns
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}