using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcare", "consent-stores", "get-iam-policy")]
public record GcloudHealthcareConsentStoresGetIamPolicyOptions(
[property: PositionalArgument] string ConsentStore,
[property: PositionalArgument] string Dataset,
[property: PositionalArgument] string Location
) : GcloudOptions;