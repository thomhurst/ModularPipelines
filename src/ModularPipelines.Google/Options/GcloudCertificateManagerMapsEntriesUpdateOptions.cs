using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("certificate-manager", "maps", "entries", "update")]
public record GcloudCertificateManagerMapsEntriesUpdateOptions(
[property: PositionalArgument] string Entry,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Map
) : GcloudOptions;