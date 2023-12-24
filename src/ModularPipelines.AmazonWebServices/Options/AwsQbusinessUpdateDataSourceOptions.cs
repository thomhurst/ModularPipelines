using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qbusiness", "update-data-source")]
public record AwsQbusinessUpdateDataSourceOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--data-source-id")] string DataSourceId,
[property: CommandSwitch("--index-id")] string IndexId
) : AwsOptions
{
    [CommandSwitch("--configuration")]
    public string? Configuration { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--document-enrichment-configuration")]
    public string? DocumentEnrichmentConfiguration { get; set; }

    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--sync-schedule")]
    public string? SyncSchedule { get; set; }

    [CommandSwitch("--vpc-configuration")]
    public string? VpcConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}