using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("topic", "command-conventions")]
public record GcloudTopicCommandConventionsOptions : GcloudOptions;