using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "service-accounts", "describe")]
public record GcloudIamServiceAccountsDescribeOptions(
[property: CliArgument] string ServiceAccount
) : GcloudOptions;