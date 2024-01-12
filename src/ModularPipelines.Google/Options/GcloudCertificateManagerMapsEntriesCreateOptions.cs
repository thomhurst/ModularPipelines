using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("certificate-manager", "maps", "entries", "create")]
public record GcloudCertificateManagerMapsEntriesCreateOptions(
[property: PositionalArgument] string Entry,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Map,
[property: CommandSwitch("--hostname")] string Hostname,
[property: BooleanCommandSwitch("--set-primary")] bool SetPrimary
) : GcloudOptions;