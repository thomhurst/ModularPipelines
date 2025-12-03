using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("service-directory", "namespaces", "set-iam-policy")]
public record GcloudServiceDirectoryNamespacesSetIamPolicyOptions(
[property: CliArgument] string Namespace,
[property: CliArgument] string Location,
[property: CliArgument] string PolicyFile
) : GcloudOptions;