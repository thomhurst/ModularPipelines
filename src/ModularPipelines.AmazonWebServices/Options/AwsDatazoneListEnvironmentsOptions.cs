using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "list-environments")]
public record AwsDatazoneListEnvironmentsOptions(
[property: CliOption("--domain-identifier")] string DomainIdentifier,
[property: CliOption("--project-identifier")] string ProjectIdentifier
) : AwsOptions
{
    [CliOption("--aws-account-id")]
    public string? AwsAccountId { get; set; }

    [CliOption("--aws-account-region")]
    public string? AwsAccountRegion { get; set; }

    [CliOption("--environment-blueprint-identifier")]
    public string? EnvironmentBlueprintIdentifier { get; set; }

    [CliOption("--environment-profile-identifier")]
    public string? EnvironmentProfileIdentifier { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--provider")]
    public string? Provider { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}