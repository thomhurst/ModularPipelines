using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sns", "set-subscription-attributes")]
public record AwsSnsSetSubscriptionAttributesOptions(
[property: CliOption("--subscription-arn")] string SubscriptionArn,
[property: CliOption("--attribute-name")] string AttributeName
) : AwsOptions
{
    [CliOption("--attribute-value")]
    public string? AttributeValue { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}