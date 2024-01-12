using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcare", "hl7v2-stores", "describe")]
public record GcloudHealthcareHl7v2StoresDescribeOptions(
[property: PositionalArgument] string HL7V2Store,
[property: PositionalArgument] string Dataset,
[property: PositionalArgument] string Location
) : GcloudOptions;