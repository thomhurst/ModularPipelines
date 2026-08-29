# Pipeline Interface Audit

This document records the result of the context-interface consolidation completed
for v4.

## Public context hierarchy

```text
IPipelineContext
├── IModuleContext
└── IModuleHookContext
```

`IPipelineContext` is the shared capability surface. `IModuleContext` adds
module-execution operations, while `IModuleHookContext` adds module lifecycle
information.

Pipeline global hooks, requirements, and run conditions use `IPipelineContext`.
Module lifecycle hooks use `IModuleHookContext`.

## Capability interfaces

Capabilities are grouped under `ModularPipelines.Context.Domains`:

- `Shell`: `ICommandContext`, `IBashContext`, `IPowerShellContext`
- `Files`: `IZipContext`
- `Data`: `IJsonContext`, `IXmlContext`, `IYamlContext`, `IBase64Context`,
  `IHexContext`
- `Environment`: `IEnvironmentVariablesContext`, `IBuildSystemContext`
- `Installers`: `IInstallersContext` for generic local and web installers
- `Network`: `IHttpContext`, `IDownloaderContext`
- `Security`: `ICertificatesContext`, `IHashContext`

The duplicate root-namespace capability interfaces and the empty pipeline-hook
marker were removed. Each capability now has one public name.

## Extension points

- `IPipelineGlobalHooks`: pipeline start and end callbacks
- `IModuleEventReceiver`: module lifecycle callbacks
- `IPipelineRequirement`: startup requirement checks
- `IRunCondition`: reusable execution conditions
- `IPipelineValidator`: custom pipeline validation

Engine orchestration interfaces remain internal implementation details.

For usage guidance and examples, see
[Interface Hierarchy](./interface-hierarchy.md).
