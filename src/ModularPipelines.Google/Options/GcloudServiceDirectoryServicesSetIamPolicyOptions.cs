using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("service-directory", "services", "set-iam-policy")]
public record GcloudServiceDirectoryServicesSetIamPolicyOptions(
[property: PositionalArgument] string Service,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Namespace,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;