using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53", "create-cidr-collection")]
public record AwsRoute53CreateCidrCollectionOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--caller-reference")] string CallerReference
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}