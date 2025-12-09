using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime", "create-bot")]
public record AwsChimeCreateBotOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--display-name")] string DisplayName
) : AwsOptions
{
    [CliOption("--domain")]
    public string? Domain { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}