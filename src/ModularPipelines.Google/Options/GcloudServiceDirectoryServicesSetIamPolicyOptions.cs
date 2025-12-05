using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("service-directory", "services", "set-iam-policy")]
public record GcloudServiceDirectoryServicesSetIamPolicyOptions(
[property: CliArgument] string Service,
[property: CliArgument] string Location,
[property: CliArgument] string Namespace,
[property: CliArgument] string PolicyFile
) : GcloudOptions;