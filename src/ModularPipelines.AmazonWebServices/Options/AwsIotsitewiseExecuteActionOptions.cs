using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotsitewise", "execute-action")]
public record AwsIotsitewiseExecuteActionOptions(
[property: CommandSwitch("--target-resource")] string TargetResource,
[property: CommandSwitch("--action-definition-id")] string ActionDefinitionId,
[property: CommandSwitch("--action-payload")] string ActionPayload
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}