using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "projects", "link")]
public record GcloudBillingProjectsLinkOptions(
[property: PositionalArgument] string ProjectId,
[property: CommandSwitch("--billing-account")] string BillingAccount
) : GcloudOptions;