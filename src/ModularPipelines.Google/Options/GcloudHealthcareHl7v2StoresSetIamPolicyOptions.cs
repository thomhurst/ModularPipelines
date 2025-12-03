using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare", "hl7v2-stores", "set-iam-policy")]
public record GcloudHealthcareHl7v2StoresSetIamPolicyOptions(
[property: CliArgument] string HL7V2Store,
[property: CliArgument] string Dataset,
[property: CliArgument] string Location,
[property: CliArgument] string PolicyFile
) : GcloudOptions;