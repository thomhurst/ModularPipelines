using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("globalaccelerator", "list-cross-account-resources")]
public record AwsGlobalacceleratorListCrossAccountResourcesOptions(
[property: CommandSwitch("--resource-owner-aws-account-id")] string ResourceOwnerAwsAccountId
) : AwsOptions
{
    [CommandSwitch("--accelerator-arn")]
    public string? AcceleratorArn { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}