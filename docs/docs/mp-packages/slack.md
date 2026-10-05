---
title: Slack Package
---

# Slack Package

Send webhook messages through `context.Tools.Slack`.

## Installation

```shell
dotnet add package ModularPipelines.Slack
```

## Send a message

Import `ModularPipelines.Slack` for the integration and its options. Inside an
asynchronous module, pass the module cancellation token:

```csharp
using ModularPipelines.Slack;
using Slack.Webhooks;

await context.Tools.Slack.PostMessageAsync(
    new SlackWebHookOptions(
        new SlackMessage { Text = "Build passed" },
        webhookUri),
    cancellationToken);
```

`webhookUri` is the webhook `Uri` obtained from your configuration. Treat it as a
secret. Cancellation stops the pending HTTP request. An unsuccessful HTTP response
throws `PipelineHttpResponseException`; the helper disposes the request and response.

## V4 migration

Replace `PostWebHookMessage` with `PostMessageAsync`. Replace imports of
`ModularPipelines.Slack.Options` and `ModularPipelines.Slack.Extensions` with
`ModularPipelines.Slack`. There are no forwarding methods for the old API.
`RegisterSlackContext` remains public for integration registration, but is hidden
from IntelliSense with `EditorBrowsable(Never)`.
