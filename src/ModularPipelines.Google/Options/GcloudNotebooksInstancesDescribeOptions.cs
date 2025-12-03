using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("notebooks", "instances", "describe")]
public record GcloudNotebooksInstancesDescribeOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Location
) : GcloudOptions;