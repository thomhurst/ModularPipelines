using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataplex", "lakes", "get-iam-policy")]
public record GcloudDataplexLakesGetIamPolicyOptions(
[property: PositionalArgument] string Lake,
[property: PositionalArgument] string Location
) : GcloudOptions;