using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "put-parameter")]
public record AwsSsmPutParameterOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--value")] string Value
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--key-id")]
    public string? KeyId { get; set; }

    [CliOption("--allowed-pattern")]
    public string? AllowedPattern { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--tier")]
    public string? Tier { get; set; }

    [CliOption("--policies")]
    public string? Policies { get; set; }

    [CliOption("--data-type")]
    public string? DataType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}