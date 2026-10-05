using ModularPipelines.Chocolatey.Options;
using ModularPipelines.Secrets;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Chocolatey.UnitTests;

public class ChocolateyGlobalOptionsTests : TestBase
{
    [Test]
    public async Task API_Key_Alias_Renders_A_Value_And_Has_Secret_Metadata()
    {
        var rendered = await RenderCommand(new ChocoApikeyOptions { Key = "example-key" });

        await Assert.That(rendered).IsEqualTo("choco apikey --key=example-key");
        await Assert.That(typeof(ChocoApikeyOptions).GetProperty(nameof(ChocoApikeyOptions.Key))!
            .IsDefined(typeof(SecretValueAttribute), inherit: true)).IsTrue();
    }

    [Test]
    public async Task Install_Renders_Common_Options_After_Package_Operand()
    {
        var rendered = await RenderCommand(new ChocoInstallOptions("example")
        {
            Debug = true,
            ProxyPassword = "example-password",
            Timeout = "60",
            Yes = true,
        });

        await Assert.That(rendered).IsEqualTo(
            "choco install example --debug --proxy-password=example-password --timeout=60 --yes");
        var password = typeof(ChocoInstallOptions).GetProperty(nameof(ChocoOptions.ProxyPassword))!;
        await Assert.That(password.DeclaringType).IsEqualTo(typeof(ChocoOptions));
        await Assert.That(password.IsDefined(typeof(SecretValueAttribute), inherit: true)).IsTrue();
    }
}
