using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "associate-mac-sec-key")]
public record AwsDirectconnectAssociateMacSecKeyOptions(
[property: CliOption("--connection-id")] string ConnectionId
) : AwsOptions
{
    [CliOption("--secret-arn")]
    public string? SecretArn { get; set; }

    [CliOption("--ckn")]
    public string? Ckn { get; set; }

    [CliOption("--cak")]
    public string? Cak { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}