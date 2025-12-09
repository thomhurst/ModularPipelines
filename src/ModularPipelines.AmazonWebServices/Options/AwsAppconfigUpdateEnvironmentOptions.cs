using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appconfig", "update-environment")]
public record AwsAppconfigUpdateEnvironmentOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--environment-id")] string EnvironmentId
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--monitors")]
    public string[]? Monitors { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}