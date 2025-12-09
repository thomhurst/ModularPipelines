using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "list-environment-profiles")]
public record AwsDatazoneListEnvironmentProfilesOptions(
[property: CliOption("--domain-identifier")] string DomainIdentifier
) : AwsOptions
{
    [CliOption("--aws-account-id")]
    public string? AwsAccountId { get; set; }

    [CliOption("--aws-account-region")]
    public string? AwsAccountRegion { get; set; }

    [CliOption("--environment-blueprint-identifier")]
    public string? EnvironmentBlueprintIdentifier { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--project-identifier")]
    public string? ProjectIdentifier { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}