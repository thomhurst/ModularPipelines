using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm-contacts", "update-contact")]
public record AwsSsmContactsUpdateContactOptions(
[property: CliOption("--contact-id")] string ContactId
) : AwsOptions
{
    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--plan")]
    public string? Plan { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}