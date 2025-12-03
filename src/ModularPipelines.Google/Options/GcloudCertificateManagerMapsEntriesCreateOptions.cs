using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("certificate-manager", "maps", "entries", "create")]
public record GcloudCertificateManagerMapsEntriesCreateOptions(
[property: CliArgument] string Entry,
[property: CliArgument] string Location,
[property: CliArgument] string Map,
[property: CliOption("--hostname")] string Hostname,
[property: CliFlag("--set-primary")] bool SetPrimary
) : GcloudOptions;