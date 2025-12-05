using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "tasks", "get-iam-policy")]
public record GcloudDataplexTasksGetIamPolicyOptions(
[property: CliArgument] string Task,
[property: CliArgument] string Lake,
[property: CliArgument] string Location
) : GcloudOptions;