# MPCLI002

Value-less bool? CliOption should use CliFlag.

| Property         | Value                   |
| ---------------- | ----------------------- |
| Category         | Usage                   |
| Default severity | Error                   |
| Availability     | Public analyzer package |

## Configure severity[​](#configure-severity "Direct link to Configure severity")

.editorconfig

```
dotnet_diagnostic.MPCLI002.severity = error
```

Use `none` to disable the rule, or `silent`, `suggestion`, `warning`, or `error` to change its severity.

## Suppress a specific occurrence[​](#suppress-a-specific-occurrence "Direct link to Suppress a specific occurrence")

```
#pragma warning disable MPCLI002

// Code that intentionally violates MPCLI002.

#pragma warning restore MPCLI002
```
