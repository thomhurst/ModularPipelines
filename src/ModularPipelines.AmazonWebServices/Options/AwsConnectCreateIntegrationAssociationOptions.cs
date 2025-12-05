using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "create-integration-association")]
public record AwsConnectCreateIntegrationAssociationOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--integration-type")] string IntegrationType,
[property: CliOption("--integration-arn")] string IntegrationArn
) : AwsOptions
{
    [CliOption("--source-application-url")]
    public string? SourceApplicationUrl { get; set; }

    [CliOption("--source-application-name")]
    public string? SourceApplicationName { get; set; }

    [CliOption("--source-type")]
    public string? SourceType { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}