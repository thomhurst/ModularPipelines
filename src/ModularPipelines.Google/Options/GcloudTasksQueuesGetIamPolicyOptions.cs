using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("tasks", "queues", "get-iam-policy")]
public record GcloudTasksQueuesGetIamPolicyOptions(
[property: CliArgument] string Queue,
[property: CliArgument] string Location
) : GcloudOptions;