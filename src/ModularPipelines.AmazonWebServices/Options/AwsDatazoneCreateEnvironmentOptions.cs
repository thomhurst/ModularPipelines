using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "create-environment")]
public record AwsDatazoneCreateEnvironmentOptions(
[property: CliOption("--domain-identifier")] string DomainIdentifier,
[property: CliOption("--environment-profile-identifier")] string EnvironmentProfileIdentifier,
[property: CliOption("--name")] string Name,
[property: CliOption("--project-identifier")] string ProjectIdentifier
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--glossary-terms")]
    public string[]? GlossaryTerms { get; set; }

    [CliOption("--user-parameters")]
    public string[]? UserParameters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}