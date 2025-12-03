using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare", "consent-stores", "get-iam-policy")]
public record GcloudHealthcareConsentStoresGetIamPolicyOptions(
[property: CliArgument] string ConsentStore,
[property: CliArgument] string Dataset,
[property: CliArgument] string Location
) : GcloudOptions;