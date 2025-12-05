using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediastore", "describe-container")]
public record AwsMediastoreDescribeContainerOptions : AwsOptions
{
    [CliOption("--container-name")]
    public string? ContainerName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}