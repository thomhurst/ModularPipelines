using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qbusiness", "update-data-source")]
public record AwsQbusinessUpdateDataSourceOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--data-source-id")] string DataSourceId,
[property: CliOption("--index-id")] string IndexId
) : AwsOptions
{
    [CliOption("--configuration")]
    public string? Configuration { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--document-enrichment-configuration")]
    public string? DocumentEnrichmentConfiguration { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--sync-schedule")]
    public string? SyncSchedule { get; set; }

    [CliOption("--vpc-configuration")]
    public string? VpcConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}