using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "create-integration-association")]
public record AwsConnectCreateIntegrationAssociationOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--integration-type")] string IntegrationType,
[property: CommandSwitch("--integration-arn")] string IntegrationArn
) : AwsOptions
{
    [CommandSwitch("--source-application-url")]
    public string? SourceApplicationUrl { get; set; }

    [CommandSwitch("--source-application-name")]
    public string? SourceApplicationName { get; set; }

    [CommandSwitch("--source-type")]
    public string? SourceType { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}