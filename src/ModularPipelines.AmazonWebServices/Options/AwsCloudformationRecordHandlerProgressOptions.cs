using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "record-handler-progress")]
public record AwsCloudformationRecordHandlerProgressOptions(
[property: CliOption("--bearer-token")] string BearerToken,
[property: CliOption("--operation-status")] string OperationStatus
) : AwsOptions
{
    [CliOption("--current-operation-status")]
    public string? CurrentOperationStatus { get; set; }

    [CliOption("--status-message")]
    public string? StatusMessage { get; set; }

    [CliOption("--error-code")]
    public string? ErrorCode { get; set; }

    [CliOption("--resource-model")]
    public string? ResourceModel { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}