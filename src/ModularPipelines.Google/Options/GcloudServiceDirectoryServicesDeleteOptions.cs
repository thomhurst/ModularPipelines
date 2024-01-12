using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("service-directory", "services", "delete")]
public record GcloudServiceDirectoryServicesDeleteOptions(
[property: PositionalArgument] string Service,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Namespace
) : GcloudOptions;