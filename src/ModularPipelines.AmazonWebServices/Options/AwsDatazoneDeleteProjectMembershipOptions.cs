using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "delete-project-membership")]
public record AwsDatazoneDeleteProjectMembershipOptions(
[property: CliOption("--domain-identifier")] string DomainIdentifier,
[property: CliOption("--member")] string Member,
[property: CliOption("--project-identifier")] string ProjectIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}