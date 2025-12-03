using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workbench", "instances", "get-iam-policy")]
public record GcloudWorkbenchInstancesGetIamPolicyOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Location
) : GcloudOptions;