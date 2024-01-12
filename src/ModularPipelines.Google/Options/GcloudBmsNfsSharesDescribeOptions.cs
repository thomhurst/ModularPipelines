using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bms", "nfs-shares", "describe")]
public record GcloudBmsNfsSharesDescribeOptions(
[property: PositionalArgument] string NfsShare,
[property: PositionalArgument] string Region
) : GcloudOptions;