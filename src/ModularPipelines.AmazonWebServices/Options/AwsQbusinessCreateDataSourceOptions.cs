using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qbusiness", "create-data-source")]
public record AwsQbusinessCreateDataSourceOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--configuration")] string Configuration,
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--index-id")] string IndexId
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--document-enrichment-configuration")]
    public string? DocumentEnrichmentConfiguration { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--sync-schedule")]
    public string? SyncSchedule { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--vpc-configuration")]
    public string? VpcConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}