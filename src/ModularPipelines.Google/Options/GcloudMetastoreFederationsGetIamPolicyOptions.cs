using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("metastore", "federations", "get-iam-policy")]
public record GcloudMetastoreFederationsGetIamPolicyOptions(
[property: CliArgument] string Federation,
[property: CliArgument] string Location
) : GcloudOptions;