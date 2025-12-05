using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bigtable", "instances", "tables", "set-iam-policy")]
public record GcloudBigtableInstancesTablesSetIamPolicyOptions(
[property: CliArgument] string Table,
[property: CliArgument] string Instance,
[property: CliArgument] string PolicyFile
) : GcloudOptions;