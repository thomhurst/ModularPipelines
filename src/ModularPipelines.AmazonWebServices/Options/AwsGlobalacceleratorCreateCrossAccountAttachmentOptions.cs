using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("globalaccelerator", "create-cross-account-attachment")]
public record AwsGlobalacceleratorCreateCrossAccountAttachmentOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--principals")]
    public string[]? Principals { get; set; }

    [CliOption("--resources")]
    public string[]? Resources { get; set; }

    [CliOption("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}