---
title: Logging and HTTP
sidebar_position: 11
---

# Logging and HTTP

## Logging

### Renamed options

| V3 | V4 |
| --- | --- |
| `CommandExecutionOptions.LogSettings` | `CommandExecutionOptions.Logging` |
| `CommandLoggingOptions.IncludeTimestamps` | `CommandLoggingOptions.ShowTimestamps` |
| `CommandLogVerbosity.Minimal` (documented as "errors and warnings") | `CommandLogVerbosity.InputOnly`, which logs command input only. Choose the level by the output you need |
| `HttpOptions.LogSettings` | `HttpOptions.Logging` |
| `HttpOptions.LoggingType` / `HttpLoggingType` flags | `HttpLoggingOptions` booleans or presets |
| `HttpLoggingOptions.LogRequest`, `LogRequestBody`, `LogStatusCode`, and the other `Log*` switches | Same names with a `Show` prefix: `ShowRequest`, `ShowRequestBody`, `ShowStatusCode`, etc. |
| `HttpLoggingOptions.None` / `Full` presets | `HttpLoggingOptions.Silent` / `Diagnostic` |
| `DownloadOptions.LoggingType` | `DownloadOptions.Logging` with `HttpLoggingOptions` |
| `context.Summary.Info(...)` | `context.Summary.Information(...)` |

- **HTTP bodies are no longer logged by default.** V3's `LogRequestBody` and `LogResponseBody`
  defaulted to `true`. `ShowRequestBody` and `ShowResponseBody` default to `false`. Set them, or
  use `HttpLoggingOptions.Diagnostic`, to log bodies.
- **Command `Show*` switches are `bool?`.** On `CommandLoggingOptions`, `null` means the switch
  follows `Verbosity`, and an explicit value overrides it in either direction. Code that reads one
  as a plain `bool` needs a null check.

### Logger and console

| V3 | V4 |
| --- | --- |
| `context.Logger` typed as `IModuleLogger` | Standard `ILogger`; rich console output goes through `context.Console` |
| `IConsoleWriter.LogToConsole(markup)` | `context.Console.WriteMarkupLine(markup)`; use `WriteLine` for literal text |
| `IModuleLoggerProvider.GetLogger()` | Resolve `IModuleLoggerAccessor` and read its `Logger` property |
| `ISummaryLogger.GetEntries(...)` / `GetOutput()` | Resolve `ISummaryLogReader` and call them there |

- Normal `ILogger` calls stay as they are.
- For module-aware console output, use `context.Console.WriteLine`, `WriteMarkupLine`, or
  `Write(IRenderable)`. Console output and structured logging both take part in module grouping
  and secret masking.
- `IConsoleWriter` now lives in `ModularPipelines.Logging`. Injected writer implementations must
  provide `WriteLine`, `WriteMarkupLine`, and `Write`.
- Do not dispose `context.Logger` yourself.
- `IHttpLogger`, `ICommandLogger`, and `IExceptionOutputFormatter` are no longer public. Configure
  output through the logging options above.

## HTTP

### Sending requests

`context.Network.Http.HttpClient` and `GetLoggingHttpClient(...)` are removed. Send requests
through `SendAsync`, so pipeline logging, timeout, and resilience settings apply:

```csharp
using var response = await context.Network.Http.SendAsync(
    new HttpOptions(new HttpRequestMessage(HttpMethod.Get, endpoint))
    {
        Logging = HttpLoggingOptions.Minimal,
        Timeout = TimeSpan.FromSeconds(30),
    },
    cancellationToken);
```

- Responses stream their bodies. Dispose responses, and pass cancellation tokens to body-reading
  operations too.
- Use `HttpLoggingOptions.Silent` to turn off HTTP logging for one request. The `Show*` switches
  are described in [HTTP logging](../how-to/logging.md#http-logging).
- For a custom client, inject one through your application DI and pass it in
  `HttpOptions.HttpClient`. The pipeline's retry handler applies only to the default client, so
  configure retries on a custom client yourself.

### Failure responses now throw

`HttpOptions.ThrowOnNonSuccessStatusCode` now defaults to `true`, matching commands and downloads.
This also applies to the implicit string, `Uri`, and `HttpRequestMessage` conversions to `HttpOptions`.

- A non-success response throws `PipelineHttpResponseException`. It has the status code, and an
  unredacted `ResponseContent` preview when available.
- Response bodies are left out of the exception *message*, including with
  `HttpLoggingOptions.Diagnostic`. To display them, with registered secrets masked, turn on HTTP
  response body logging explicitly.

If your pipeline handles failure responses on purpose, opt out:

```csharp
using var response = await context.Network.Http.SendAsync(
    new HttpOptions(new HttpRequestMessage(HttpMethod.Get, endpoint))
    {
        ThrowOnNonSuccessStatusCode = false,
    },
    cancellationToken);
```

Download helpers still require a successful response. Use `SendAsync` with the opt-out when you
need to inspect a failure response body.

### Retries

:::warning `POST` and `PATCH` are no longer retried by default
V3 retried all HTTP methods. Audit `POST`/`PATCH` calls that relied on that.
:::

The default client's policy:

| Aspect | Default |
| --- | --- |
| Retried methods | `GET`, `HEAD`, `PUT`, `DELETE`, `OPTIONS`, `TRACE` |
| Sent once unless opted in | `POST`, `PATCH`, and custom methods |
| Attempts | Three retries after the initial attempt, with exponential backoff and jitter |
| Retried failures | Network failures, timeout failures, and status codes `408`, `500`, `502`, `503`, `504` |

**Request bodies.** A request with a body can be retried only when its content is
`ByteArrayContent` (which includes `StringContent` and `FormUrlEncodedContent`) with a known length
of at most 1 MiB (1,048,576 bytes). Streamed content, other content types, unknown lengths, and
larger bodies are sent once without replay buffering, even for `PUT` or with an explicit opt-in.
Replay buffering also enforces the limit against the actual serialized body. If that exceeds the
limit, the request falls back to a single send.

**Opting in.** Set the per-request `HttpResilienceOptions.RetryEnabled` option only when the server
guarantees that repeating the operation is safe, for example through a supported idempotency key:

```csharp
using var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
{
    Content = new StringContent(payload),
};
// The endpoint must support this key; reuse it for the same logical operation.
request.Headers.Add("Idempotency-Key", operationId);
request.Options.Set(HttpResilienceOptions.RetryEnabled, true);

using var response = await context.Network.Http.SendAsync(request, cancellationToken);
```

- `RetryEnabled = false` turns off retries for one request, including an otherwise retryable method.
- `RetryEnabled = true` overrides only the method default. It cannot bypass the body restrictions
  or a global `MaxRetryAttempts = 0` (also available as `HttpResilienceOptions.None`).

**Configuring the policy.** Use `PipelineOptions.Http.Resilience`, or the `ConfigureHttp` shortcut:

```csharp
builder.ConfigureHttp(http => http with
{
    Timeout = TimeSpan.FromSeconds(30),
    Resilience = new HttpResilienceOptions
    {
        MaxRetryAttempts = 3,
        AttemptTimeout = TimeSpan.FromSeconds(5),
        RetryOnTimeout = true,
    },
});
```

### Timeouts

**`AttemptTimeout`** is a per-attempt deadline.

- It defaults to `null`, which adds no per-attempt deadline. Use `null` to turn it off.
- When set, it must be positive and no greater than 4,294,967,294 milliseconds.
- It covers sending the request and receiving response headers, not reading the response body.
- Each attempt gets a fresh deadline. That includes the initial send when retries are off or the
  request cannot be retried.
- An expired attempt can be retried when `RetryOnTimeout` is `true` and the request is eligible.
  Setting it to `false` prevents timeout retries.

**The overall timeout** still bounds the whole retry sequence, including backoff. So does caller
cancellation. Neither is retried.

- `HttpOptions.Timeout` overrides `PipelineOptions.Http.Timeout`. Otherwise the client's timeout
  supplies the limit.
- A short overall timeout can end the request before all configured retries run.

These resilience settings, including `AttemptTimeout`, apply only to the default client. Configure
resilience yourself when you supply `HttpOptions.HttpClient`.
