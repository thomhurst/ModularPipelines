using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("service-directory", "namespaces", "add-iam-policy-binding")]
public record GcloudServiceDirectoryNamespacesAddIamPolicyBindingOptions(
[property: CliArgument] string Namespace,
[property: CliArgument] string Location,
[property: CliOption("--member")] string Member,
[property: CliOption("--role")] string Role
) : GcloudOptions;