using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ram", "get-resource-shares")]
public record AwsRamGetResourceSharesOptions(
[property: CommandSwitch("--resource-owner")] string ResourceOwner
) : AwsOptions
{
    [CommandSwitch("--resource-share-arns")]
    public string[]? ResourceShareArns { get; set; }

    [CommandSwitch("--resource-share-status")]
    public string? ResourceShareStatus { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--tag-filters")]
    public string[]? TagFilters { get; set; }

    [CommandSwitch("--permission-arn")]
    public string? PermissionArn { get; set; }

    [CommandSwitch("--permission-version")]
    public int? PermissionVersion { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}