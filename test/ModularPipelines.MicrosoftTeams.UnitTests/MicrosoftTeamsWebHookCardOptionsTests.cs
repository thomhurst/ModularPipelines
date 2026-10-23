namespace ModularPipelines.MicrosoftTeams.UnitTests;

public class MicrosoftTeamsWebHookCardOptionsTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ToStringRedactsWebhookUri(bool clone)
    {
        var options = new MicrosoftTeamsWebHookCardOptions(
            new MicrosoftTeamsAdaptiveCard(),
            new Uri("https://example.test/webhook/original-secret?token=original-token"));
        if (clone)
        {
            options = options with
            {
                WebHookUri = new Uri("https://example.test/webhook/cloned-secret?token=cloned-token"),
                ThrowOnNonSuccessStatusCode = false,
            };
        }

        var output = options.ToString();

        await Assert.That(output).DoesNotContain(options.WebHookUri.AbsoluteUri);
        await Assert.That(output).DoesNotContain("secret");
        await Assert.That(output).DoesNotContain("token");
        await Assert.That(output).Contains("WebHookUri = <redacted>");
        await Assert.That(output).Contains("Card = ");
        await Assert.That(output).Contains($"ThrowOnNonSuccessStatusCode = {options.ThrowOnNonSuccessStatusCode}");
        await Assert.That(typeof(MicrosoftTeamsWebHookCardOptions).IsSealed).IsTrue();
    }
}
