using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagegateway", "delete-chap-credentials")]
public record AwsStoragegatewayDeleteChapCredentialsOptions(
[property: CommandSwitch("--target-arn")] string TargetArn,
[property: CommandSwitch("--initiator-name")] string InitiatorName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}