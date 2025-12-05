using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bms", "nfs-shares", "describe")]
public record GcloudBmsNfsSharesDescribeOptions(
[property: CliArgument] string NfsShare,
[property: CliArgument] string Region
) : GcloudOptions;