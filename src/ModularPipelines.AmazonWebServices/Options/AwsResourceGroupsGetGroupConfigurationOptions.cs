using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-groups", "get-group-configuration")]
public record AwsResourceGroupsGetGroupConfigurationOptions : AwsOptions
{
    [CliOption("--group")]
    public string? Group { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}