using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bigtable", "instances", "delete")]
public record GcloudBigtableInstancesDeleteOptions(
[property: CliArgument] string Instance
) : GcloudOptions;