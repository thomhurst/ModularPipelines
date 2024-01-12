using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet", "cloudrun", "enable")]
public record GcloudContainerFleetCloudrunEnableOptions : GcloudOptions;