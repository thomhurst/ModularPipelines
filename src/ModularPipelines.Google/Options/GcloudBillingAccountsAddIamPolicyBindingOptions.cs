using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "accounts", "add-iam-policy-binding")]
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

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string GcloudBillingAccountsAddIamPolicyBindingOptionsAccount { get; set; }

    [CommandSwitch("--member")]
    public string Member { get; set; }

    [CommandSwitch("--role")]
    public string Role { get; set; }
}
