using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "update-hours-of-operation")]
public record AwsConnectUpdateHoursOfOperationOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--hours-of-operation-id")] string HoursOfOperationId
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--time-zone")]
    public string? TimeZone { get; set; }

    [CliOption("--config")]
    public string[]? Config { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}