using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("service-directory", "namespaces", "get-iam-policy")]
public record GcloudServiceDirectoryNamespacesGetIamPolicyOptions(
[property: PositionalArgument] string Namespace,
[property: PositionalArgument] string Location
) : GcloudOptions;