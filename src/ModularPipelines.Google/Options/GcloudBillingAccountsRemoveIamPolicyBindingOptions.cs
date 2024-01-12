using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "accounts", "remove-iam-policy-binding")]
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

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string GcloudBillingAccountsRemoveIamPolicyBindingOptionsAccount { get; set; }

    [CommandSwitch("--member")]
    public string Member { get; set; }

    [CommandSwitch("--role")]
    public string Role { get; set; }
}
