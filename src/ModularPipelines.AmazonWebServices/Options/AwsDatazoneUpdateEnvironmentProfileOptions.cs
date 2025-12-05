using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "update-environment-profile")]
public record AwsDatazoneUpdateEnvironmentProfileOptions(
[property: CliOption("--domain-identifier")] string DomainIdentifier,
[property: CliOption("--identifier")] string Identifier
) : AwsOptions
{
    [CliOption("--aws-account-id")]
    public string? AwsAccountId { get; set; }

    [CliOption("--aws-account-region")]
    public string? AwsAccountRegion { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--user-parameters")]
    public string[]? UserParameters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}