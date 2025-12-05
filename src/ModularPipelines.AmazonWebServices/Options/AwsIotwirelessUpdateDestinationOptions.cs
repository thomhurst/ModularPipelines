using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "update-destination")]
public record AwsIotwirelessUpdateDestinationOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--expression-type")]
    public string? ExpressionType { get; set; }

    [CliOption("--expression")]
    public string? Expression { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}