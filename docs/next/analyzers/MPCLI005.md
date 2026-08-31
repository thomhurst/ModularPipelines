# MPCLI005

Negated CLI flag applied to a non-nullable-Boolean or counted property.

| Property         | Value                   |
| ---------------- | ----------------------- |
| Category         | Usage                   |
| Default severity | Error                   |
| Availability     | Public analyzer package |

## Configure severity[​](#configure-severity "Direct link to Configure severity")

.editorconfig

```
dotnet_diagnostic.MPCLI005.severity = error
```

Use `none` to disable the rule, or `silent`, `suggestion`, `warning`, or `error` to change its severity.

## Suppress a specific occurrence[​](#suppress-a-specific-occurrence "Direct link to Suppress a specific occurrence")

```
#pragma warning disable MPCLI005

// Code that intentionally violates MPCLI005.

#pragma warning restore MPCLI005
```
