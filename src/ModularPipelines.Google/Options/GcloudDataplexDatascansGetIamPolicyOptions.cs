using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataplex", "datascans", "get-iam-policy")]
public record GcloudDataplexDatascansGetIamPolicyOptions(
[property: PositionalArgument] string Datascan,
[property: PositionalArgument] string Location
) : GcloudOptions;