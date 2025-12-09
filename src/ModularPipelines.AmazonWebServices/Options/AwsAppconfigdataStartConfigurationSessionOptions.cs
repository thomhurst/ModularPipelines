using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appconfigdata", "start-configuration-session")]
public record AwsAppconfigdataStartConfigurationSessionOptions(
[property: CliOption("--application-identifier")] string ApplicationIdentifier,
[property: CliOption("--environment-identifier")] string EnvironmentIdentifier,
[property: CliOption("--configuration-profile-identifier")] string ConfigurationProfileIdentifier
) : AwsOptions
{
    [CliOption("--required-minimum-poll-interval-in-seconds")]
    public int? RequiredMinimumPollIntervalInSeconds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}