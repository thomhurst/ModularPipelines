using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("topic", "flags-file")]
public record GcloudTopicFlagsFileOptions : GcloudOptions;