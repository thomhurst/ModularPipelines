using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare", "fhir-stores", "set-iam-policy")]
public record GcloudHealthcareFhirStoresSetIamPolicyOptions(
[property: CliArgument] string FhirStore,
[property: CliArgument] string Dataset,
[property: CliArgument] string Location,
[property: CliArgument] string PolicyFile
) : GcloudOptions;