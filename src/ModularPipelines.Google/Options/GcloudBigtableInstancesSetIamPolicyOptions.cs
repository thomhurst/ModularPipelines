using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bigtable", "instances", "set-iam-policy")]
public record GcloudBigtableInstancesSetIamPolicyOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string PolicyFile
) : GcloudOptions;