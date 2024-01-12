using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("auth", "application-default", "set-quota-project")]
public record GcloudAuthApplicationDefaultSetQuotaProjectOptions(
[property: PositionalArgument] string QuotaProjectId
) : GcloudOptions;