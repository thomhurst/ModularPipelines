using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "delete-integration-association")]
public record AwsConnectDeleteIntegrationAssociationOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--integration-association-id")] string IntegrationAssociationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}