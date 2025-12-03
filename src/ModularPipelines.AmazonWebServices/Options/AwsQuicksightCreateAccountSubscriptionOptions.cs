using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "create-account-subscription")]
public record AwsQuicksightCreateAccountSubscriptionOptions(
[property: CliOption("--edition")] string Edition,
[property: CliOption("--authentication-method")] string AuthenticationMethod,
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--account-name")] string AccountName,
[property: CliOption("--notification-email")] string NotificationEmail
) : AwsOptions
{
    [CliOption("--active-directory-name")]
    public string? ActiveDirectoryName { get; set; }

    [CliOption("--realm")]
    public string? Realm { get; set; }

    [CliOption("--directory-id")]
    public string? DirectoryId { get; set; }

    [CliOption("--admin-group")]
    public string[]? AdminGroup { get; set; }

    [CliOption("--author-group")]
    public string[]? AuthorGroup { get; set; }

    [CliOption("--reader-group")]
    public string[]? ReaderGroup { get; set; }

    [CliOption("--first-name")]
    public string? FirstName { get; set; }

    [CliOption("--last-name")]
    public string? LastName { get; set; }

    [CliOption("--email-address")]
    public string? EmailAddress { get; set; }

    [CliOption("--contact-number")]
    public string? ContactNumber { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}