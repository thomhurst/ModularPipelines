using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "delete-integration-association")]
public record AwsConnectDeleteIntegrationAssociationOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--integration-association-id")] string IntegrationAssociationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}