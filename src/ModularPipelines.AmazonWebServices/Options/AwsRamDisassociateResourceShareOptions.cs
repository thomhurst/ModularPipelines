using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ram", "disassociate-resource-share")]
public record AwsRamDisassociateResourceShareOptions(
[property: CliOption("--resource-share-arn")] string ResourceShareArn
) : AwsOptions
{
    [CliOption("--resource-arns")]
    public string[]? ResourceArns { get; set; }

    [CliOption("--principals")]
    public string[]? Principals { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--sources")]
    public string[]? Sources { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}