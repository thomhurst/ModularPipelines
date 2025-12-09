using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm-contacts", "accept-page")]
public record AwsSsmContactsAcceptPageOptions(
[property: CliOption("--page-id")] string PageId,
[property: CliOption("--accept-type")] string AcceptType,
[property: CliOption("--accept-code")] string AcceptCode
) : AwsOptions
{
    [CliOption("--contact-channel-id")]
    public string? ContactChannelId { get; set; }

    [CliOption("--note")]
    public string? Note { get; set; }

    [CliOption("--accept-code-validation")]
    public string? AcceptCodeValidation { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}