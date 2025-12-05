using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("metastore", "federations", "add-iam-policy-binding")]
public record GcloudMetastoreFederationsAddIamPolicyBindingOptions(
[property: CliArgument] string Federation,
[property: CliArgument] string Location,
[property: CliOption("--member")] string Member,
[property: CliOption("--role")] string Role
) : GcloudOptions;