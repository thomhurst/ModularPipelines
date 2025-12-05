using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("migrationhuborchestrator", "list-workflows")]
public record AwsMigrationhuborchestratorListWorkflowsOptions : AwsOptions
{
    [CliOption("--template-id")]
    public string? TemplateId { get; set; }

    [CliOption("--ads-application-configuration-name")]
    public string? AdsApplicationConfigurationName { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}