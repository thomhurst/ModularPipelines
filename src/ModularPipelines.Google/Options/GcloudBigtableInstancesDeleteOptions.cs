using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bigtable", "instances", "delete")]
public record GcloudBigtableInstancesDeleteOptions(
[property: PositionalArgument] string Instance
) : GcloudOptions;