using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ram", "list-permission-associations")]
public record AwsRamListPermissionAssociationsOptions : AwsOptions
{
    [CommandSwitch("--permission-arn")]
    public string? PermissionArn { get; set; }

    [CommandSwitch("--permission-version")]
    public int? PermissionVersion { get; set; }

    [CommandSwitch("--association-status")]
    public string? AssociationStatus { get; set; }

    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }

    [CommandSwitch("--feature-set")]
    public string? FeatureSet { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}