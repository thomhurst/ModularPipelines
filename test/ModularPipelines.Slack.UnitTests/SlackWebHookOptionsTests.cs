namespace ModularPipelines.Slack.UnitTests;

public class SlackWebHookOptionsTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ToStringRedactsWebhookUri(bool clone)
    {
        var options = new SlackWebHookOptions(
            new global::Slack.Webhooks.SlackMessage { Text = "Build passed" },
            new Uri("https://example.test/webhook/original-secret?token=original-token"));
        if (clone)
        {
            options = options with { WebHookUri = new Uri("https://example.test/webhook/cloned-secret?token=cloned-token") };
        }

        var output = options.ToString();

        await Assert.That(output).DoesNotContain(options.WebHookUri.AbsoluteUri);
        await Assert.That(output).DoesNotContain("secret");
        await Assert.That(output).DoesNotContain("token");
        await Assert.That(output).Contains("WebHookUri = <redacted>");
        await Assert.That(output).Contains("SlackMessage = ");
        await Assert.That(typeof(SlackWebHookOptions).IsSealed).IsTrue();
    }
}
