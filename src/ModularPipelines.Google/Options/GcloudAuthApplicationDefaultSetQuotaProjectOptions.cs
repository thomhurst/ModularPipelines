using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auth", "application-default", "set-quota-project")]
public record GcloudAuthApplicationDefaultSetQuotaProjectOptions(
[property: CliArgument] string QuotaProjectId
) : GcloudOptions;