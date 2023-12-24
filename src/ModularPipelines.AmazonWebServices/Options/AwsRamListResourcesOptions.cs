using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ram", "list-resources")]
public record AwsRamListResourcesOptions(
[property: CommandSwitch("--resource-owner")] string ResourceOwner
) : AwsOptions
{
    [CommandSwitch("--principal")]
    public string? Principal { get; set; }

    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }

    [CommandSwitch("--resource-arns")]
    public string[]? ResourceArns { get; set; }

    [CommandSwitch("--resource-share-arns")]
    public string[]? ResourceShareArns { get; set; }

    [CommandSwitch("--resource-region-scope")]
    public string? ResourceRegionScope { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}