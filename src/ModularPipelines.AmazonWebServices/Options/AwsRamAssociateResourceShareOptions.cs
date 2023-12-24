using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ram", "associate-resource-share")]
public record AwsRamAssociateResourceShareOptions(
[property: CommandSwitch("--resource-share-arn")] string ResourceShareArn
) : AwsOptions
{
    [CommandSwitch("--resource-arns")]
    public string[]? ResourceArns { get; set; }

    [CommandSwitch("--principals")]
    public string[]? Principals { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--sources")]
    public string[]? Sources { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}