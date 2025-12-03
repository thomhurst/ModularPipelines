using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qbusiness", "create-application")]
public record AwsQbusinessCreateApplicationOptions(
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--attachments-configuration")]
    public string? AttachmentsConfiguration { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--encryption-configuration")]
    public string? EncryptionConfiguration { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}