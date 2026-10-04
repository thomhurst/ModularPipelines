---
title: Microsoft Teams Package
---

# Microsoft Teams Package

Send adaptive cards through `context.Tools.MicrosoftTeams`.

## Installation

```shell
dotnet add package ModularPipelines.MicrosoftTeams
```

## Send a card

Import `ModularPipelines.MicrosoftTeams` for the integration, options, and card
models. Inside an asynchronous module, pass the module cancellation token:

```csharp
using ModularPipelines.MicrosoftTeams;

using var response = await context.Tools.MicrosoftTeams.PostCardAsync(
    new MicrosoftTeamsWebHookCardOptions(
        new MicrosoftTeamsAdaptiveCard
        {
            MsTeams = new MicrosoftTeamsProperties { Width = "Full" },
        },
        webhookUri),
    cancellationToken);
```

`webhookUri` is the webhook `Uri` obtained from your configuration. Treat it as a
secret. Add your content to the adaptive card before posting. Cancellation stops
the pending HTTP request. The caller owns and must dispose the returned response.

Unsuccessful HTTP responses throw `PipelineHttpResponseException` by default.
To inspect an unsuccessful response yourself, set
`ThrowOnNonSuccessStatusCode = false` on `MicrosoftTeamsWebHookCardOptions`.

## V4 migration

Replace `PostMicrosoftTeamsCard` with `PostCardAsync`. Replace imports of
`ModularPipelines.MicrosoftTeams.Options`, `.Models`, and `.Extensions` with
`ModularPipelines.MicrosoftTeams`. There are no forwarding methods for the old API.
`RegisterMicrosoftTeamsContext` remains public for integration registration, but
is hidden from IntelliSense with `EditorBrowsable(Never)`.
