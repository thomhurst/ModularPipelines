using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("topic", "ignore")]
public record GcloudTopicGcloudignoreOptions : GcloudOptions;