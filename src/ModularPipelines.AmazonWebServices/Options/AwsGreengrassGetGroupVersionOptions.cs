using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("greengrass", "get-group-version")]
public record AwsGreengrassGetGroupVersionOptions(
[property: CliOption("--group-id")] string GroupId,
[property: CliOption("--group-version-id")] string GroupVersionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}