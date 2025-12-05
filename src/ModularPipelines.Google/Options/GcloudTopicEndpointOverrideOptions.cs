using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("topic", "endpoint-override")]
public record GcloudTopicEndpointOverrideOptions : GcloudOptions;