using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "jobs", "describe")]
public record GcloudTransferJobsDescribeOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;