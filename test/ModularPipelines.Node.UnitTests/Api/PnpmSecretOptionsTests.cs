using ModularPipelines.Secrets;
using ModularPipelines.Attributes;
using ModularPipelines.Node.Options;

namespace ModularPipelines.Node.UnitTests.Api;

public class PnpmSecretOptionsTests
{
    [Test]
    [Arguments(typeof(PnpmPublishOptions))]
    [Arguments(typeof(PnpmStageOptions))]
    public async Task Otp_Is_Marked_As_Secret(Type optionsType)
    {
        var otpProperty = optionsType.GetProperty(nameof(PnpmPublishOptions.Otp));

        await Assert.That(otpProperty).IsNotNull();
        await Assert.That(otpProperty!.IsDefined(typeof(SecretValueAttribute), inherit: true)).IsTrue();
    }
}
