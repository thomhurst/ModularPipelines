using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("notebooks", "instances", "get-iam-policy")]
public record GcloudNotebooksInstancesGetIamPolicyOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string Location
) : GcloudOptions;