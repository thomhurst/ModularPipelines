using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("service-directory", "namespaces", "get-iam-policy")]
public record GcloudServiceDirectoryNamespacesGetIamPolicyOptions(
[property: CliArgument] string Namespace,
[property: CliArgument] string Location
) : GcloudOptions;