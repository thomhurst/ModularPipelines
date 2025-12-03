using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("metastore", "federations", "set-iam-policy")]
public record GcloudMetastoreFederationsSetIamPolicyOptions(
[property: CliArgument] string Federation,
[property: CliArgument] string Location,
[property: CliArgument] string PolicyFile
) : GcloudOptions;