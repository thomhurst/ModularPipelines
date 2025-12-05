using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagegateway", "join-domain")]
public record AwsStoragegatewayJoinDomainOptions(
[property: CliOption("--gateway-arn")] string GatewayArn,
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--user-name")] string UserName,
[property: CliOption("--password")] string Password
) : AwsOptions
{
    [CliOption("--organizational-unit")]
    public string? OrganizationalUnit { get; set; }

    [CliOption("--domain-controllers")]
    public string[]? DomainControllers { get; set; }

    [CliOption("--timeout-in-seconds")]
    public int? TimeoutInSeconds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}