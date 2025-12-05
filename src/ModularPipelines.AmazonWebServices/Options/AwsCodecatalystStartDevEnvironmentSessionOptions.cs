using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecatalyst", "start-dev-environment-session")]
public record AwsCodecatalystStartDevEnvironmentSessionOptions(
[property: CliOption("--space-name")] string SpaceName,
[property: CliOption("--project-name")] string ProjectName,
[property: CliOption("--id")] string Id,
[property: CliOption("--session-configuration")] string SessionConfiguration
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}