using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "projects", "list")]
public record GcloudBillingProjectsListOptions(
[property: CommandSwitch("--billing-account")] string BillingAccount
) : GcloudOptions;