using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("identitystore", "create-user")]
public record AwsIdentitystoreCreateUserOptions(
[property: CliOption("--identity-store-id")] string IdentityStoreId
) : AwsOptions
{
    [CliOption("--user-name")]
    public string? UserName { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--nick-name")]
    public string? NickName { get; set; }

    [CliOption("--profile-url")]
    public string? ProfileUrl { get; set; }

    [CliOption("--emails")]
    public string[]? Emails { get; set; }

    [CliOption("--addresses")]
    public string[]? Addresses { get; set; }

    [CliOption("--phone-numbers")]
    public string[]? PhoneNumbers { get; set; }

    [CliOption("--user-type")]
    public string? UserType { get; set; }

    [CliOption("--title")]
    public string? Title { get; set; }

    [CliOption("--preferred-language")]
    public string? PreferredLanguage { get; set; }

    [CliOption("--locale")]
    public string? Locale { get; set; }

    [CliOption("--timezone")]
    public string? Timezone { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}