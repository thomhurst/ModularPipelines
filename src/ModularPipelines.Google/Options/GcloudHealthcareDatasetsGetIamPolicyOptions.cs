using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare", "datasets", "get-iam-policy")]
public record GcloudHealthcareDatasetsGetIamPolicyOptions(
[property: CliArgument] string Dataset,
[property: CliArgument] string Location
) : GcloudOptions;