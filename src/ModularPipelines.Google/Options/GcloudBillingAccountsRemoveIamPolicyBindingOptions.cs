using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing", "accounts", "remove-iam-policy-binding")]
public record GcloudBillingAccountsRemoveIamPolicyBindingOptions : GcloudOptions
{
    public GcloudBillingAccountsRemoveIamPolicyBindingOptions(
        string account,
        string member,
        string role
    )
    {
        GcloudBillingAccountsRemoveIamPolicyBindingOptionsAccount = account;
        Member = member;
        Role = role;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string GcloudBillingAccountsRemoveIamPolicyBindingOptionsAccount { get; set; }

    [CliOption("--member")]
    public string Member { get; set; }

    [CliOption("--role")]
    public string Role { get; set; }
}
