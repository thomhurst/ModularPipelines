using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appconfigdata", "start-configuration-session")]
public record AwsAppconfigdataStartConfigurationSessionOptions(
[property: CommandSwitch("--application-identifier")] string ApplicationIdentifier,
[property: CommandSwitch("--environment-identifier")] string EnvironmentIdentifier,
[property: CommandSwitch("--configuration-profile-identifier")] string ConfigurationProfileIdentifier
) : AwsOptions
{
    [CommandSwitch("--required-minimum-poll-interval-in-seconds")]
    public int? RequiredMinimumPollIntervalInSeconds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}