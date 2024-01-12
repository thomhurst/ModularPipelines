using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("topic", "resource-keys")]
public record GcloudTopicResourceKeysOptions : GcloudOptions;