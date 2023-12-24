using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qbusiness", "create-data-source")]
public record AwsQbusinessCreateDataSourceOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--configuration")] string Configuration,
[property: CommandSwitch("--display-name")] string DisplayName,
[property: CommandSwitch("--index-id")] string IndexId
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--document-enrichment-configuration")]
    public string? DocumentEnrichmentConfiguration { get; set; }

    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--sync-schedule")]
    public string? SyncSchedule { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--vpc-configuration")]
    public string? VpcConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}