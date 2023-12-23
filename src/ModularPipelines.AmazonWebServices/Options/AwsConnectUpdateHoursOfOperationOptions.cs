using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "update-hours-of-operation")]
public record AwsConnectUpdateHoursOfOperationOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--hours-of-operation-id")] string HoursOfOperationId
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--time-zone")]
    public string? TimeZone { get; set; }

    [CommandSwitch("--config")]
    public string[]? Config { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}