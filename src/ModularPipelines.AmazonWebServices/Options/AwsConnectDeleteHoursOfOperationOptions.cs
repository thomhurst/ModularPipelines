using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "delete-hours-of-operation")]
public record AwsConnectDeleteHoursOfOperationOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--hours-of-operation-id")] string HoursOfOperationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}