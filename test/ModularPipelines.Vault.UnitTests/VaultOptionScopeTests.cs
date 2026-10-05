using ModularPipelines.Secrets;
using ModularPipelines.TestHelpers;
using ModularPipelines.Vault.Options;

namespace ModularPipelines.Vault.UnitTests;

public class VaultOptionScopeTests : TestBase
{
    [Test]
    public async Task Http_Settings_Follow_Command_And_Precede_Path()
    {
        var rendered = await RenderCommand(new VaultReadOptions("secret/example")
        {
            Address = "https://vault.example",
            Namespace = "engineering",
            Format = "json",
        });

        await Assert.That(rendered).IsEqualTo(
            "vault read -address=https://vault.example -namespace=engineering -format=json secret/example");
    }

    [Test]
    public async Task Headers_And_Mfa_Repeat_Their_Switches()
    {
        var rendered = await RenderCommand(new VaultReadOptions("secret/example")
        {
            Header = ["X-First=one", "X-Second=two"],
            Mfa = ["method-one:111111", "method-two:222222"],
        });

        await Assert.That(rendered).IsEqualTo(
            "vault read -header=X-First=one -header=X-Second=two -mfa=method-one:111111 -mfa=method-two:222222 secret/example");
    }

    [Test]
    [Arguments(null)]
    [Arguments(false)]
    public async Task Unset_Or_False_Http_Flags_Are_Omitted(bool? value)
    {
        await Assert.That(await RenderCommand(new VaultReadOptions("secret/example") { TlsSkipVerify = value }))
            .IsEqualTo("vault read secret/example");
    }

    [Test]
    public async Task Http_Credentials_Are_Masked_And_Client_Key_Paths_Remain_Visible()
    {
        var options = new VaultReadOptions("secret/example")
        {
            Header = ["Authorization=header-secret"],
            Mfa = ["method:mfa-secret"],
            UnlockKey = "unlock-secret",
            ClientKey = "client-key.pem",
        };
        var rendered = await RenderCommand(options);
        var obfuscator = await GetService<ISecretObfuscator>();
        var masked = obfuscator.Obfuscate(rendered, options);

        await Assert.That(masked).DoesNotContain("header-secret");
        await Assert.That(masked).DoesNotContain("mfa-secret");
        await Assert.That(masked).DoesNotContain("unlock-secret");
        await Assert.That(masked).Contains("-client-key=client-key.pem");
    }

    [Test]
    public async Task Token_Create_Id_Is_Rendered_And_Masked()
    {
        var options = new VaultTokenCreateOptions
        {
            Id = "hvs.explicit-token-credential",
            DisplayName = "deployment",
        };
        var rendered = await RenderCommand(options);
        var obfuscator = await GetService<ISecretObfuscator>();
        var masked = obfuscator.Obfuscate(rendered, options);

        await Assert.That(rendered).Contains("-id=hvs.explicit-token-credential");
        await Assert.That(masked).DoesNotContain("hvs.explicit-token-credential");
        await Assert.That(masked).Contains("-display-name=deployment");
    }

    [Test]
    public async Task Login_Authentication_Operands_Are_Masked()
    {
        var options = new VaultLoginOptions { AuthKV = ["hvs.example-token"] };
        var rendered = await RenderCommand(options);
        var obfuscator = await GetService<ISecretObfuscator>();

        await Assert.That(rendered).IsEqualTo("vault login hvs.example-token");
        await Assert.That(obfuscator.Obfuscate(rendered, options)).DoesNotContain("hvs.example-token");
    }

    [Test]
    public async Task Root_Decoding_Credentials_Are_Masked_And_Public_Key_Path_Is_Visible()
    {
        var options = new VaultOperatorGenerateRootOptions
        {
            Decode = "encoded-root-token",
            Otp = "root-otp-secret",
            PgpKey = "public-key.asc",
        };
        var rendered = await RenderCommand(options);
        var obfuscator = await GetService<ISecretObfuscator>();
        var masked = obfuscator.Obfuscate(rendered, options);

        await Assert.That(masked).DoesNotContain("encoded-root-token");
        await Assert.That(masked).DoesNotContain("root-otp-secret");
        await Assert.That(masked).Contains("-pgp-key=public-key.asc");
    }

    [Test]
    public async Task Http_And_Output_Options_Are_Not_Promoted_Universally()
    {
        await Assert.That(typeof(VaultOptions).GetProperty("Address")).IsNull();
        await Assert.That(typeof(VaultPrintOptions).GetProperty("Address")).IsNull();
        await Assert.That(typeof(VaultServerOptions).GetProperty("Format")).IsNull();
        await Assert.That(await RenderCommand(new VaultPrintOptions("token"))).IsEqualTo("vault print token");
    }

    [Test]
    public async Task Server_Root_Token_Is_Command_Local_And_Masked()
    {
        var options = new VaultServerOptions { Dev = true, DevRootTokenId = "example-root-token" };
        var rendered = await RenderCommand(options);
        var obfuscator = await GetService<ISecretObfuscator>();

        await Assert.That(rendered).IsEqualTo("vault server -dev -dev-root-token-id=example-root-token");
        await Assert.That(obfuscator.Obfuscate(rendered, options)).DoesNotContain("example-root-token");
        await Assert.That(typeof(VaultReadOptions).GetProperty("DevRootTokenId")).IsNull();
    }
}
