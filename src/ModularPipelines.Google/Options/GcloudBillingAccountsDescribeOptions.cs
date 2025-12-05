using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing", "accounts", "describe")]
public record GcloudBillingAccountsDescribeOptions(
[property: CliArgument] string AccountId
) : GcloudOptions;