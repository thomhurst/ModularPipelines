using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "cloudrun", "enable")]
public record GcloudContainerHubCloudrunEnableOptions : GcloudOptions;