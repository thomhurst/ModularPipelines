using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudformation", "list-types")]
public record AwsCloudformationListTypesOptions : AwsOptions
{
    [CommandSwitch("--visibility")]
    public string? Visibility { get; set; }

    [CommandSwitch("--provisioning-type")]
    public string? ProvisioningType { get; set; }

    [CommandSwitch("--deprecated-status")]
    public string? DeprecatedStatus { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--filters")]
    public string? Filters { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}