using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "create-account-subscription")]
public record AwsQuicksightCreateAccountSubscriptionOptions(
[property: CommandSwitch("--edition")] string Edition,
[property: CommandSwitch("--authentication-method")] string AuthenticationMethod,
[property: CommandSwitch("--aws-account-id")] string AwsAccountId,
[property: CommandSwitch("--account-name")] string AccountName,
[property: CommandSwitch("--notification-email")] string NotificationEmail
) : AwsOptions
{
    [CommandSwitch("--active-directory-name")]
    public string? ActiveDirectoryName { get; set; }

    [CommandSwitch("--realm")]
    public string? Realm { get; set; }

    [CommandSwitch("--directory-id")]
    public string? DirectoryId { get; set; }

    [CommandSwitch("--admin-group")]
    public string[]? AdminGroup { get; set; }

    [CommandSwitch("--author-group")]
    public string[]? AuthorGroup { get; set; }

    [CommandSwitch("--reader-group")]
    public string[]? ReaderGroup { get; set; }

    [CommandSwitch("--first-name")]
    public string? FirstName { get; set; }

    [CommandSwitch("--last-name")]
    public string? LastName { get; set; }

    [CommandSwitch("--email-address")]
    public string? EmailAddress { get; set; }

    [CommandSwitch("--contact-number")]
    public string? ContactNumber { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}