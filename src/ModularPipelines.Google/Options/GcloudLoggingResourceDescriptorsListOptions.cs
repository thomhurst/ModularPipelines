using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logging", "resource-descriptors", "list")]
public record GcloudLoggingResourceDescriptorsListOptions : GcloudOptions;