using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("service-directory", "services", "get-iam-policy")]
public record GcloudServiceDirectoryServicesGetIamPolicyOptions(
[property: CliArgument] string Service,
[property: CliArgument] string Location,
[property: CliArgument] string Namespace
) : GcloudOptions;