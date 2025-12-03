using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare", "consent-stores", "set-iam-policy")]
public record GcloudHealthcareConsentStoresSetIamPolicyOptions(
[property: CliArgument] string ConsentStore,
[property: CliArgument] string Dataset,
[property: CliArgument] string Location,
[property: CliArgument] string PolicyFile
) : GcloudOptions;