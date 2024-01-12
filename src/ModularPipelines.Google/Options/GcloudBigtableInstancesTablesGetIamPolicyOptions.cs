using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bigtable", "instances", "tables", "get-iam-policy")]
public record GcloudBigtableInstancesTablesGetIamPolicyOptions(
[property: PositionalArgument] string Table,
[property: PositionalArgument] string Instance
) : GcloudOptions;