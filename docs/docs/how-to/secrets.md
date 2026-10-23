---
title: Secrets
---

# Secrets

## Use IOptions\<\>
If you don't know how IOptions work, then go check out a tutorial. This is the recommended and supported way of storing configuration in Modular Pipelines.

Your options classes should be registered as IOptions.

If you have any sensitive/secret data stored in these classes, you can attribute your property with `[SecretValue]`.

This attribute, combined with the logger exposed by `context.Logger`, means that if that value is ever attempted to be written to logs, it'll be censored out, so that secret values aren't visible to those unauthorised.

`[SecretValue]` supports scalar strings, character sequences such as `char[]`,
`IEnumerable<char>`, `Memory<char>`, and `ReadOnlyMemory<char>`, and collections of
secret values. Character sequences are treated as one secret, while secret
collections are masked item by item.

## Example

```csharp
public record MySettings
{
    [SecretValue]
    public string? ApiKey { get; set; }
}
```

## Register secrets discovered at runtime

Inside a module, register secrets returned by a vault, authentication service, or API through
`context.Security.Secrets` before those values can reach logs or command output:

```csharp
var apiKey = await GetApiKeyFromVaultAsync(cancellationToken);
context.Security.Secrets.AddSecret(apiKey);

// Subsequent pipeline logging masks the registered value.
context.Logger.LogInformation("Using API key: {ApiKey}", apiKey);
```

Use `AddSecrets(...)` to register several values at once. Registration is thread-safe and uses
this pipeline's existing `ISecretRegistry`, so constructor injection remains supported and all
modules in the same pipeline share the registered values. Separate pipelines have separate
registries. Configured masking options and supported CI-native masking still apply.

Register values before logging them: registration does not redact output that has already been
written. Null, empty, and whitespace-only values are ignored.

## Exceptions leaving the pipeline

`IPipeline.RunAsync()` masks registered secrets in exceptions before returning them to the caller.
When masking changes exception text or diagnostics, the pipeline throws a sanitized snapshot with
masked messages, stack traces, and inner exceptions, including every aggregate exception branch.
The snapshot remains masked after the pipeline is disposed, so unhandled exception output also
uses the masked text. The original exception is not exposed through `InnerException`.

Structured or opaque `Exception.Data` keys and values become text snapshots so their original
objects cannot expose unmasked contents. This conservatively requires a sanitized exception even
when the object's text contains no secret. Plain scalar values retain their types when unchanged.

Sanitized exceptions can have a different concrete type from the original exception. For example,
`ModuleFailedException` and `PipelineFailedException` are replaced, so catches for those types and
access to `PipelineFailedException.Summary` do not apply to sanitized exceptions. Aggregate
exceptions remain `AggregateException` instances, and cancellation remains an
`OperationCanceledException` with the original cancellation token. When nothing needs masking,
the original exception and its type are preserved. Register secrets before throwing an exception
that contains them; the same masking settings used for logs apply.

## Mask a configuration section

When a configuration provider supplies a group of secrets, you can register every leaf value in
that section without adding `[SecretValue]` to an options class:

```csharp
var builder = Pipeline.CreateBuilder();

builder.MaskConfigurationSection("Secrets");
```

Nested values such as `Secrets:Database:Password` are included. Missing sections and empty values
are ignored. The values are registered during pipeline startup and use the same log and CI-native
masking as `[SecretValue]` and `ISecretRegistry`.

Masking itself is not replaceable: console output relies on the built-in masker to hide secrets
that are split across separate writes. Supply secrets through `[SecretValue]`, `ISecretRegistry`,
or `SecretMaskingOptions` instead.

You can also configure multiple sections through options:

```csharp
builder.ConfigureSecrets(secrets => secrets with
{
    MaskedConfigurationSections = ["Secrets", "ConnectionStrings"],
});
```
