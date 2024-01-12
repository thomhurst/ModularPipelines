using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bigtable", "instances", "get-iam-policy")]
public record GcloudBigtableInstancesGetIamPolicyOptions(
[property: PositionalArgument] string Instance
) : GcloudOptions;