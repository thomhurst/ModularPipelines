using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ram", "get-resource-share-associations")]
public record AwsRamGetResourceShareAssociationsOptions(
[property: CommandSwitch("--association-type")] string AssociationType
) : AwsOptions
{
    [CommandSwitch("--resource-share-arns")]
    public string[]? ResourceShareArns { get; set; }

    [CommandSwitch("--resource-arn")]
    public string? ResourceArn { get; set; }

    [CommandSwitch("--principal")]
    public string? Principal { get; set; }

    [CommandSwitch("--association-status")]
    public string? AssociationStatus { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}