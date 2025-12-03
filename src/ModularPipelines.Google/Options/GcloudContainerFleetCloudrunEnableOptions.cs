using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "cloudrun", "enable")]
public record GcloudContainerFleetCloudrunEnableOptions : GcloudOptions;