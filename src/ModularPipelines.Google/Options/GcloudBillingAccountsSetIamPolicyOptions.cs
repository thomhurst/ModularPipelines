using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing", "accounts", "set-iam-policy")]
public record GcloudBillingAccountsSetIamPolicyOptions : GcloudOptions
{
    public GcloudBillingAccountsSetIamPolicyOptions(
        string account,
        string policyFile
    )
    {
        GcloudBillingAccountsSetIamPolicyOptionsAccount = account;
        PolicyFile = policyFile;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string GcloudBillingAccountsSetIamPolicyOptionsAccount { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string PolicyFile { get; set; }
}
