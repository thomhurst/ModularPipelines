using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "simulate-principal-policy")]
public record AwsIamSimulatePrincipalPolicyOptions(
[property: CommandSwitch("--policy-source-arn")] string PolicySourceArn,
[property: CommandSwitch("--action-names")] string[] ActionNames
) : AwsOptions
{
    [CommandSwitch("--policy-input-list")]
    public string[]? PolicyInputList { get; set; }

    [CommandSwitch("--permissions-boundary-policy-input-list")]
    public string[]? PermissionsBoundaryPolicyInputList { get; set; }

    [CommandSwitch("--resource-arns")]
    public string[]? ResourceArns { get; set; }

    [CommandSwitch("--resource-policy")]
    public string? ResourcePolicy { get; set; }

    [CommandSwitch("--resource-owner")]
    public string? ResourceOwner { get; set; }

    [CommandSwitch("--caller-arn")]
    public string? CallerArn { get; set; }

    [CommandSwitch("--context-entries")]
    public string[]? ContextEntries { get; set; }

    [CommandSwitch("--resource-handling-option")]
    public string? ResourceHandlingOption { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}