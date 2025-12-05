using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "create-destination")]
public record AwsIotwirelessCreateDestinationOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--expression-type")] string ExpressionType,
[property: CliOption("--expression")] string Expression,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}