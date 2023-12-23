using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "delete-use-case")]
public record AwsConnectDeleteUseCaseOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--integration-association-id")] string IntegrationAssociationId,
[property: CommandSwitch("--use-case-id")] string UseCaseId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}