using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("notebooks", "instances", "add-iam-policy-binding")]
public record GcloudNotebooksInstancesAddIamPolicyBindingOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Location,
[property: CliOption("--member")] string Member,
[property: CliOption("--role")] string Role
) : GcloudOptions;