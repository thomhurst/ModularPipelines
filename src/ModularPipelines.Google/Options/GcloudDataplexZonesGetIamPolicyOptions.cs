using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataplex", "zones", "get-iam-policy")]
public record GcloudDataplexZonesGetIamPolicyOptions(
[property: PositionalArgument] string Zone,
[property: PositionalArgument] string Lake,
[property: PositionalArgument] string Location
) : GcloudOptions;