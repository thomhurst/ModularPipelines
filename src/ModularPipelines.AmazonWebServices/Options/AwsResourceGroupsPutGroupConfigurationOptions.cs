using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-groups", "put-group-configuration")]
public record AwsResourceGroupsPutGroupConfigurationOptions : AwsOptions
{
    [CliOption("--group")]
    public string? Group { get; set; }

    [CliOption("--configuration")]
    public string[]? Configuration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}