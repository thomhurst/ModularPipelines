using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("metastore", "services", "set-iam-policy")]
public record GcloudMetastoreServicesSetIamPolicyOptions(
[property: CliArgument] string Service,
[property: CliArgument] string Location,
[property: CliArgument] string PolicyFile
) : GcloudOptions;