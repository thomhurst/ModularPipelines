using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("service-directory", "namespaces", "remove-iam-policy-binding")]
public record GcloudServiceDirectoryNamespacesRemoveIamPolicyBindingOptions(
[property: CliArgument] string Namespace,
[property: CliArgument] string Location,
[property: CliOption("--member")] string Member,
[property: CliOption("--role")] string Role
) : GcloudOptions;