using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bigtable", "instances", "get-iam-policy")]
public record GcloudBigtableInstancesGetIamPolicyOptions(
[property: CliArgument] string Instance
) : GcloudOptions;