using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "zones", "add-iam-policy-binding")]
public record GcloudDataplexZonesAddIamPolicyBindingOptions(
[property: CliArgument] string Zone,
[property: CliArgument] string Lake,
[property: CliArgument] string Location,
[property: CliOption("--member")] string Member,
[property: CliOption("--role")] string Role
) : GcloudOptions;