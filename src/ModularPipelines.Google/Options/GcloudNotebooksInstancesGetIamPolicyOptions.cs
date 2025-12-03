using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("notebooks", "instances", "get-iam-policy")]
public record GcloudNotebooksInstancesGetIamPolicyOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Location
) : GcloudOptions;