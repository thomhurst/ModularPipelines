using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudformation", "record-handler-progress")]
public record AwsCloudformationRecordHandlerProgressOptions(
[property: CommandSwitch("--bearer-token")] string BearerToken,
[property: CommandSwitch("--operation-status")] string OperationStatus
) : AwsOptions
{
    [CommandSwitch("--current-operation-status")]
    public string? CurrentOperationStatus { get; set; }

    [CommandSwitch("--status-message")]
    public string? StatusMessage { get; set; }

    [CommandSwitch("--error-code")]
    public string? ErrorCode { get; set; }

    [CommandSwitch("--resource-model")]
    public string? ResourceModel { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}