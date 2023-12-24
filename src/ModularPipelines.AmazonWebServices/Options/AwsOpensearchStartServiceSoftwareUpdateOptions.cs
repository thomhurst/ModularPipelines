using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opensearch", "start-service-software-update")]
public record AwsOpensearchStartServiceSoftwareUpdateOptions(
[property: CommandSwitch("--domain-name")] string DomainName
) : AwsOptions
{
    [CommandSwitch("--schedule-at")]
    public string? ScheduleAt { get; set; }

    [CommandSwitch("--desired-start-time")]
    public long? DesiredStartTime { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}