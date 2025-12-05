using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bigtable", "instances", "tables", "get-iam-policy")]
public record GcloudBigtableInstancesTablesGetIamPolicyOptions(
[property: CliArgument] string Table,
[property: CliArgument] string Instance
) : GcloudOptions;