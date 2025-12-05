using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qbusiness", "delete-web-experience")]
public record AwsQbusinessDeleteWebExperienceOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--web-experience-id")] string WebExperienceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}