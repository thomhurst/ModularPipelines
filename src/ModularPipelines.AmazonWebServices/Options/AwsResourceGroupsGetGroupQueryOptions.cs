using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-groups", "get-group-query")]
public record AwsResourceGroupsGetGroupQueryOptions : AwsOptions
{
    [CliOption("--group-name")]
    public string? GroupName { get; set; }

    [CliOption("--group")]
    public string? Group { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}