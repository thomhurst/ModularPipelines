# MPCLI001

CliFlag property must be bool? or int?

| Property         | Value                   |
| ---------------- | ----------------------- |
| Category         | Usage                   |
| Default severity | Error                   |
| Availability     | Public analyzer package |

## Configure severity[​](#configure-severity "Direct link to Configure severity")

.editorconfig

```
dotnet_diagnostic.MPCLI001.severity = error
```

Use `none` to disable the rule, or `silent`, `suggestion`, `warning`, or `error` to change its severity.

## Suppress a specific occurrence[​](#suppress-a-specific-occurrence "Direct link to Suppress a specific occurrence")

```
#pragma warning disable MPCLI001

// Code that intentionally violates MPCLI001.

#pragma warning restore MPCLI001
```
