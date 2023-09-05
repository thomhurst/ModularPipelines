using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Slack.Extensions;
using ModularPipelines.Slack.Options;
using Slack.Webhooks;
using Slack.Webhooks.Blocks;
using Slack.Webhooks.Elements;
using Slack.Webhooks.Interfaces;

namespace ModularPipelines.Examples.Modules;

public class PostSlackModule : Module
{
    protected override async Task<ModuleResult<IDictionary<string, object>>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await context.Slack().PostWebHookMessage(new SlackWebHookOptions(
            new SlackMessage
            {
                Text = "Foo bar!",
                Blocks = new List<Block>
                {
                    new Section()
                    {
                        Text = new TextObject
                        {
                            Text = "Bar",
                        }
                    },
                    new Divider(),
                    new Actions()
                    {
                        Elements = new List<IActionElement>
                        {
                            new Button()
                            {
                                Text = new TextObject("Click me!")
                            }
                        }
                    },
                    new Section()
                    {
                        Text = new TextObject("Foo!")
                    }
                }
            },
            new Uri("https://hooks.slack.com/services/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")
        ));

        return await NothingAsync();
    }
}