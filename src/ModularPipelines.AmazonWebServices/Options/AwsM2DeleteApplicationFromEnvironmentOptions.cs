using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("m2", "delete-application-from-environment")]
public record AwsM2DeleteApplicationFromEnvironmentOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--environment-id")] string EnvironmentId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}