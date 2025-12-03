using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "cloudrun", "enable")]
public record GcloudContainerHubCloudrunEnableOptions : GcloudOptions;