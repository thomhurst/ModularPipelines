using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm-contacts", "create-contact-channel")]
public record AwsSsmContactsCreateContactChannelOptions(
[property: CliOption("--contact-id")] string ContactId,
[property: CliOption("--name")] string Name,
[property: CliOption("--type")] string Type,
[property: CliOption("--delivery-address")] string DeliveryAddress
) : AwsOptions
{
    [CliOption("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}