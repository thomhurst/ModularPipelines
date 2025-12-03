using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logging", "resource-descriptors", "list")]
public record GcloudLoggingResourceDescriptorsListOptions : GcloudOptions;