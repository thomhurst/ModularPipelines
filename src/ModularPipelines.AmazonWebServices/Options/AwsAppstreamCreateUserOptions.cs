using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appstream", "create-user")]
public record AwsAppstreamCreateUserOptions(
[property: CliOption("--user-name")] string UserName,
[property: CliOption("--authentication-type")] string AuthenticationType
) : AwsOptions
{
    [CliOption("--message-action")]
    public string? MessageAction { get; set; }

    [CliOption("--first-name")]
    public string? FirstName { get; set; }

    [CliOption("--last-name")]
    public string? LastName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}