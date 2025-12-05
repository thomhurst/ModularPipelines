using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare", "datasets", "set-iam-policy")]
public record GcloudHealthcareDatasetsSetIamPolicyOptions(
[property: CliArgument] string Dataset,
[property: CliArgument] string Location,
[property: CliArgument] string PolicyFile
) : GcloudOptions;