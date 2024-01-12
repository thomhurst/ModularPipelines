using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netapp", "active-directories", "describe")]
public record GcloudNetappActiveDirectoriesDescribeOptions(
[property: PositionalArgument] string ActiveDirectory,
[property: PositionalArgument] string Location
) : GcloudOptions;