using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("notebooks", "instances", "get-health")]
public record GcloudNotebooksInstancesGetHealthOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Location
) : GcloudOptions;