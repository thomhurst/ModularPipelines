using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing", "projects", "list")]
public record GcloudBillingProjectsListOptions(
[property: CliOption("--billing-account")] string BillingAccount
) : GcloudOptions;