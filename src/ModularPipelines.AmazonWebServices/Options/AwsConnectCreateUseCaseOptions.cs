using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "create-use-case")]
public record AwsConnectCreateUseCaseOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--integration-association-id")] string IntegrationAssociationId,
[property: CliOption("--use-case-type")] string UseCaseType
) : AwsOptions
{
    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}