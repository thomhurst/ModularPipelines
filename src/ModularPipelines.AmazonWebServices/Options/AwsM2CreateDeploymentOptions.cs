using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("m2", "create-deployment")]
public record AwsM2CreateDeploymentOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--application-version")] int ApplicationVersion,
[property: CliOption("--environment-id")] string EnvironmentId
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}