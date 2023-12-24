using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appconfig", "get-hosted-configuration-version")]
public record AwsAppconfigGetHostedConfigurationVersionOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--configuration-profile-id")] string ConfigurationProfileId,
[property: CommandSwitch("--version-number")] int VersionNumber
) : AwsOptions;