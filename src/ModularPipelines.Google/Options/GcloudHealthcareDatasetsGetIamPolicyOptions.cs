using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcare", "datasets", "get-iam-policy")]
public record GcloudHealthcareDatasetsGetIamPolicyOptions(
[property: PositionalArgument] string Dataset,
[property: PositionalArgument] string Location
) : GcloudOptions;