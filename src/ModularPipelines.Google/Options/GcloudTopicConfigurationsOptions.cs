using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("topic", "configurations")]
public record GcloudTopicConfigurationsOptions : GcloudOptions;