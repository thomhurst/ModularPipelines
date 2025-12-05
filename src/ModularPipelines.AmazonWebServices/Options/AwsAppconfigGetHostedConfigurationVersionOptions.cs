using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appconfig", "get-hosted-configuration-version")]
public record AwsAppconfigGetHostedConfigurationVersionOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--configuration-profile-id")] string ConfigurationProfileId,
[property: CliOption("--version-number")] int VersionNumber
) : AwsOptions;