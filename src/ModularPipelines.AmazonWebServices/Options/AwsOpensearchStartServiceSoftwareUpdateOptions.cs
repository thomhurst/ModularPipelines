using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opensearch", "start-service-software-update")]
public record AwsOpensearchStartServiceSoftwareUpdateOptions(
[property: CliOption("--domain-name")] string DomainName
) : AwsOptions
{
    [CliOption("--schedule-at")]
    public string? ScheduleAt { get; set; }

    [CliOption("--desired-start-time")]
    public long? DesiredStartTime { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}