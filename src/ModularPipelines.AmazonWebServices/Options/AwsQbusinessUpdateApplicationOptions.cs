using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qbusiness", "update-application")]
public record AwsQbusinessUpdateApplicationOptions(
[property: CliOption("--application-id")] string ApplicationId
) : AwsOptions
{
    [CliOption("--attachments-configuration")]
    public string? AttachmentsConfiguration { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}