using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "list-data-sources")]
public record AwsDatazoneListDataSourcesOptions(
[property: CliOption("--domain-identifier")] string DomainIdentifier,
[property: CliOption("--project-identifier")] string ProjectIdentifier
) : AwsOptions
{
    [CliOption("--environment-identifier")]
    public string? EnvironmentIdentifier { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}