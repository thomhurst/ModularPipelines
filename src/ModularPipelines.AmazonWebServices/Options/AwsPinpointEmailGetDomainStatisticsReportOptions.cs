using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-email", "get-domain-statistics-report")]
public record AwsPinpointEmailGetDomainStatisticsReportOptions(
[property: CommandSwitch("--domain")] string Domain,
[property: CommandSwitch("--start-date")] long StartDate,
[property: CommandSwitch("--end-date")] long EndDate
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}