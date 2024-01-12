using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "accounts", "set-iam-policy")]
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

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string GcloudBillingAccountsSetIamPolicyOptionsAccount { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string PolicyFile { get; set; }
}
