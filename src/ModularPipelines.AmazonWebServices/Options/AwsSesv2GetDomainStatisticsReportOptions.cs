using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sesv2", "get-domain-statistics-report")]
public record AwsSesv2GetDomainStatisticsReportOptions(
[property: CliOption("--domain")] string Domain,
[property: CliOption("--start-date")] long StartDate,
[property: CliOption("--end-date")] long EndDate
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}