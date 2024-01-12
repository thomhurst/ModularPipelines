using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bigtable", "instances", "tables", "set-iam-policy")]
public record GcloudBigtableInstancesTablesSetIamPolicyOptions(
[property: PositionalArgument] string Table,
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;