using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "create-use-case")]
public record AwsConnectCreateUseCaseOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--integration-association-id")] string IntegrationAssociationId,
[property: CommandSwitch("--use-case-type")] string UseCaseType
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}