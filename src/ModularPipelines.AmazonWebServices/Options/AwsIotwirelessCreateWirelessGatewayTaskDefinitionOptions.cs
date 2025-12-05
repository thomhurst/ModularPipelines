using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "create-wireless-gateway-task-definition")]
public record AwsIotwirelessCreateWirelessGatewayTaskDefinitionOptions : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--update")]
    public string? Update { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}