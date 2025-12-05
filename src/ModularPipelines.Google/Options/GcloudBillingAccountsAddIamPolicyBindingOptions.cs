using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing", "accounts", "add-iam-policy-binding")]
public record GcloudBillingAccountsAddIamPolicyBindingOptions : GcloudOptions
{
    public GcloudBillingAccountsAddIamPolicyBindingOptions(
        string account,
        string member,
        string role
    )
    {
        GcloudBillingAccountsAddIamPolicyBindingOptionsAccount = account;
        Member = member;
        Role = role;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string GcloudBillingAccountsAddIamPolicyBindingOptionsAccount { get; set; }

    [CliOption("--member")]
    public string Member { get; set; }

    [CliOption("--role")]
    public string Role { get; set; }
}
