using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "simulate-principal-policy")]
public record AwsIamSimulatePrincipalPolicyOptions(
[property: CliOption("--policy-source-arn")] string PolicySourceArn,
[property: CliOption("--action-names")] string[] ActionNames
) : AwsOptions
{
    [CliOption("--policy-input-list")]
    public string[]? PolicyInputList { get; set; }

    [CliOption("--permissions-boundary-policy-input-list")]
    public string[]? PermissionsBoundaryPolicyInputList { get; set; }

    [CliOption("--resource-arns")]
    public string[]? ResourceArns { get; set; }

    [CliOption("--resource-policy")]
    public string? ResourcePolicy { get; set; }

    [CliOption("--resource-owner")]
    public string? ResourceOwner { get; set; }

    [CliOption("--caller-arn")]
    public string? CallerArn { get; set; }

    [CliOption("--context-entries")]
    public string[]? ContextEntries { get; set; }

    [CliOption("--resource-handling-option")]
    public string? ResourceHandlingOption { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}