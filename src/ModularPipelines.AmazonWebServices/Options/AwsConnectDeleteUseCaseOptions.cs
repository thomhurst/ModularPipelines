using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "delete-use-case")]
public record AwsConnectDeleteUseCaseOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--integration-association-id")] string IntegrationAssociationId,
[property: CliOption("--use-case-id")] string UseCaseId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}