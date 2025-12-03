using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagegateway", "set-local-console-password")]
public record AwsStoragegatewaySetLocalConsolePasswordOptions(
[property: CliOption("--gateway-arn")] string GatewayArn,
[property: CliOption("--local-console-password")] string LocalConsolePassword
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}