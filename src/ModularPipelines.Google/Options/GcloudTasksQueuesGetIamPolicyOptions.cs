using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tasks", "queues", "get-iam-policy")]
public record GcloudTasksQueuesGetIamPolicyOptions(
[property: PositionalArgument] string Queue,
[property: PositionalArgument] string Location
) : GcloudOptions;