using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcare", "fhir-stores", "get-iam-policy")]
public record GcloudHealthcareFhirStoresGetIamPolicyOptions(
[property: PositionalArgument] string FhirStore,
[property: PositionalArgument] string Dataset,
[property: PositionalArgument] string Location
) : GcloudOptions;