using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "create-environment-profile")]
public record AwsDatazoneCreateEnvironmentProfileOptions(
[property: CliOption("--domain-identifier")] string DomainIdentifier,
[property: CliOption("--environment-blueprint-identifier")] string EnvironmentBlueprintIdentifier,
[property: CliOption("--name")] string Name,
[property: CliOption("--project-identifier")] string ProjectIdentifier
) : AwsOptions
{
    [CliOption("--aws-account-id")]
    public string? AwsAccountId { get; set; }

    [CliOption("--aws-account-region")]
    public string? AwsAccountRegion { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--user-parameters")]
    public string[]? UserParameters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}