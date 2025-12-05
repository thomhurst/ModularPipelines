using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("notebooks", "instances", "is-upgradeable")]
public record GcloudNotebooksInstancesIsUpgradeableOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Location
) : GcloudOptions;