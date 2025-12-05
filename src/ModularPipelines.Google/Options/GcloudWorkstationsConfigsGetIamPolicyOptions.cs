using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workstations", "configs", "get-iam-policy")]
public record GcloudWorkstationsConfigsGetIamPolicyOptions(
[property: CliArgument] string Config,
[property: CliArgument] string Cluster,
[property: CliArgument] string Region
) : GcloudOptions;