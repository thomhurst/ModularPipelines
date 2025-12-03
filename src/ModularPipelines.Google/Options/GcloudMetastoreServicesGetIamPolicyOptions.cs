using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("metastore", "services", "get-iam-policy")]
public record GcloudMetastoreServicesGetIamPolicyOptions(
[property: CliArgument] string Service,
[property: CliArgument] string Location
) : GcloudOptions;