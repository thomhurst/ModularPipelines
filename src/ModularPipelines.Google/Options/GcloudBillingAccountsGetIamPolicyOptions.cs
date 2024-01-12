using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "accounts", "get-iam-policy")]
public record GcloudBillingAccountsGetIamPolicyOptions : GcloudOptions
{
    public GcloudBillingAccountsGetIamPolicyOptions(
        string account
    )
    {
        GcloudBillingAccountsGetIamPolicyOptionsAccount = account;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string GcloudBillingAccountsGetIamPolicyOptionsAccount { get; set; }
}
