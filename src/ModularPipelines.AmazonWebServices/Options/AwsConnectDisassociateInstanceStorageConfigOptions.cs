using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "disassociate-instance-storage-config")]
public record AwsConnectDisassociateInstanceStorageConfigOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--association-id")] string AssociationId,
[property: CliOption("--resource-type")] string ResourceType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}