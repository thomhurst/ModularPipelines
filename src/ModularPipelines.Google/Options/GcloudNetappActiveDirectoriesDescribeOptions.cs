using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("netapp", "active-directories", "describe")]
public record GcloudNetappActiveDirectoriesDescribeOptions(
[property: CliArgument] string ActiveDirectory,
[property: CliArgument] string Location
) : GcloudOptions;