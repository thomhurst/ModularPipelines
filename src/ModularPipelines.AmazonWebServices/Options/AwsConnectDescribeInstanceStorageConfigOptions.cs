using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "describe-instance-storage-config")]
public record AwsConnectDescribeInstanceStorageConfigOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--association-id")] string AssociationId,
[property: CliOption("--resource-type")] string ResourceType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}