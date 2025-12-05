using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing", "projects", "link")]
public record GcloudBillingProjectsLinkOptions(
[property: CliArgument] string ProjectId,
[property: CliOption("--billing-account")] string BillingAccount
) : GcloudOptions;