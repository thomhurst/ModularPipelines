using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing", "accounts", "get-iam-policy")]
public record GcloudBillingAccountsGetIamPolicyOptions : GcloudOptions
{
    public GcloudBillingAccountsGetIamPolicyOptions(
        string account
    )
    {
        GcloudBillingAccountsGetIamPolicyOptionsAccount = account;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string GcloudBillingAccountsGetIamPolicyOptionsAccount { get; set; }
}
