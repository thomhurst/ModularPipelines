using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qbusiness", "get-web-experience")]
public record AwsQbusinessGetWebExperienceOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--web-experience-id")] string WebExperienceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}