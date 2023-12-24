using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("identitystore", "create-user")]
public record AwsIdentitystoreCreateUserOptions(
[property: CommandSwitch("--identity-store-id")] string IdentityStoreId
) : AwsOptions
{
    [CommandSwitch("--user-name")]
    public string? UserName { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--nick-name")]
    public string? NickName { get; set; }

    [CommandSwitch("--profile-url")]
    public string? ProfileUrl { get; set; }

    [CommandSwitch("--emails")]
    public string[]? Emails { get; set; }

    [CommandSwitch("--addresses")]
    public string[]? Addresses { get; set; }

    [CommandSwitch("--phone-numbers")]
    public string[]? PhoneNumbers { get; set; }

    [CommandSwitch("--user-type")]
    public string? UserType { get; set; }

    [CommandSwitch("--title")]
    public string? Title { get; set; }

    [CommandSwitch("--preferred-language")]
    public string? PreferredLanguage { get; set; }

    [CommandSwitch("--locale")]
    public string? Locale { get; set; }

    [CommandSwitch("--timezone")]
    public string? Timezone { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}