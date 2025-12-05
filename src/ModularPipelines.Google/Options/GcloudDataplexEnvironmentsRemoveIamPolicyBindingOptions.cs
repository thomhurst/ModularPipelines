using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "environments", "remove-iam-policy-binding")]
public record GcloudDataplexEnvironmentsRemoveIamPolicyBindingOptions(
[property: CliArgument] string Environment,
[property: CliArgument] string Lake,
[property: CliArgument] string Location,
[property: CliOption("--member")] string Member,
[property: CliOption("--role")] string Role
) : GcloudOptions;