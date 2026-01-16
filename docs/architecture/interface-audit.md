# IPipeline Interface Audit

This document provides a comprehensive audit of all `IPipeline*` interfaces in ModularPipelines, documenting their purpose, usage patterns, and recommendations for consolidation.

**Audit Date:** 2026-01-07
**Total Interfaces Found:** 23
**Issue:** #1867

---

## Summary

| Category | Count | Action |
|----------|-------|--------|
| User-Facing Context | 7 | Keep public, deprecate IPipelineContext |
| Internal Engine | 8 | Already internal |
| Extension Points | 4 | Keep public |
| Validation | 2 | Make IPipelineValidationService internal |
| DI Wrapper | 1 | Already internal |
| Module Context | 1 | Keep public (primary interface) |

---

## Interface Categories

### 1. User-Facing Context Interfaces (7)

#### IPipelineHookContext
- **Location:** `src/ModularPipelines/Context/IPipelineHookContext.cs`
- **Visibility:** Public
- **Inherits From:** IPipelineServices, IPipelineLogging, IPipelineTools, IPipelineEncoding, IPipelineFileSystem, IPipelineEnvironment
- **Purpose:** Base interface for hooks, provides unified access to all pipeline capabilities
- **Usages:** Used by IPipelineGlobalHooks, IPipelineModuleHooks, IPipelineRequirement
- **Recommendation:** **Keep** - Essential for hook implementations

#### IPipelineContext
- **Location:** `src/ModularPipelines/Context/IPipelineContext.cs`
- **Visibility:** Public
- **Inherits From:** IPipelineHookContext
- **Purpose:** Adds internal module access methods
- **Usages:** ~110 references across codebase (mostly internal engine and legacy code)
- **Recommendation:** **Deprecate** - Mark obsolete, recommend IModuleContext instead

#### IPipelineServices
- **Location:** `src/ModularPipelines/Context/IPipelineServices.cs`
- **Visibility:** Public
- **Members:** ServiceProvider, Get<T>(), Configuration, PipelineOptions
- **Purpose:** DI and configuration access
- **Recommendation:** **Keep** - Part of interface segregation

#### IPipelineLogging
- **Location:** `src/ModularPipelines/Context/IPipelineLogging.cs`
- **Visibility:** Public
- **Members:** Logger (IModuleLogger)
- **Purpose:** Logging access
- **Recommendation:** **Keep** - Part of interface segregation

#### IPipelineTools
- **Location:** `src/ModularPipelines/Context/IPipelineTools.cs`
- **Visibility:** Public
- **Members:** Command, Powershell, Bash, Http, Downloader, Installer
- **Purpose:** Command execution and tool helpers
- **Recommendation:** **Keep** - Part of interface segregation

#### IPipelineEncoding
- **Location:** `src/ModularPipelines/Context/IPipelineEncoding.cs`
- **Visibility:** Public
- **Members:** Json, Xml, Yaml, Hex, Base64, Hasher
- **Purpose:** Serialization and encoding helpers
- **Recommendation:** **Keep** - Part of interface segregation

#### IPipelineFileSystem
- **Location:** `src/ModularPipelines/Context/IPipelineFileSystem.cs`
- **Visibility:** Public
- **Members:** FileSystem, Zip, Checksum
- **Purpose:** File operations
- **Recommendation:** **Keep** - Part of interface segregation

#### IPipelineEnvironment
- **Location:** `src/ModularPipelines/Context/IPipelineEnvironment.cs`
- **Visibility:** Public
- **Members:** Environment, BuildSystemDetector
- **Purpose:** Environment and build system detection
- **Recommendation:** **Keep** - Part of interface segregation

---

### 2. Internal Engine Interfaces (8) - Already Internal

#### IPipelineExecutor
- **Location:** `src/ModularPipelines/Engine/Executors/IPipelineExecutor.cs`
- **Visibility:** Internal
- **Purpose:** Pipeline execution orchestration
- **Recommendation:** **Keep internal** - Implementation detail

#### IPipelineInitializer
- **Location:** `src/ModularPipelines/Engine/Executors/IPipelineInitializer.cs`
- **Visibility:** Internal
- **Purpose:** Pipeline initialization
- **Recommendation:** **Keep internal** - Implementation detail

#### IPipelineOutputCoordinator
- **Location:** `src/ModularPipelines/Engine/Executors/IPipelineOutputCoordinator.cs`
- **Visibility:** Internal
- **Purpose:** Output coordination (progress, results, exceptions)
- **Recommendation:** **Keep internal** - Implementation detail

#### IPipelineOutputScope
- **Location:** `src/ModularPipelines/Engine/Executors/IPipelineOutputScope.cs`
- **Visibility:** Internal
- **Purpose:** Output lifecycle scope
- **Recommendation:** **Keep internal** - Implementation detail

#### IPipelineSummaryFactory
- **Location:** `src/ModularPipelines/Engine/Executors/IPipelineSummaryFactory.cs`
- **Visibility:** Internal
- **Purpose:** Summary generation
- **Recommendation:** **Keep internal** - Implementation detail

#### IPipelineContextProvider
- **Location:** `src/ModularPipelines/Engine/IPipelineContextProvider.cs`
- **Visibility:** Internal
- **Purpose:** Context provisioning
- **Recommendation:** **Keep internal** - Implementation detail

#### IPipelineFileWriter
- **Location:** `src/ModularPipelines/Engine/IPipelineFileWriter.cs`
- **Visibility:** Internal
- **Purpose:** File writing operations
- **Recommendation:** **Keep internal** - Implementation detail

#### IPipelineSetupExecutor
- **Location:** `src/ModularPipelines/Engine/IPipelineSetupExecutor.cs`
- **Visibility:** Internal
- **Purpose:** Setup and hook execution
- **Recommendation:** **Keep internal** - Implementation detail

---

### 3. Extension Point Interfaces (4)

#### IPipelineHost
- **Location:** `src/ModularPipelines/Host/IPipelineHost.cs`
- **Visibility:** Public
- **Inherits From:** IHost, IAsyncDisposable
- **Purpose:** Host abstraction for pipeline execution
- **Recommendation:** **Keep public** - User-facing extension point

#### IPipelineGlobalHooks
- **Location:** `src/ModularPipelines/Interfaces/IPipelineGlobalHooks.cs`
- **Visibility:** Public
- **Members:** OnStartAsync(), OnEndAsync()
- **Purpose:** Global pipeline lifecycle hooks
- **Recommendation:** **Keep public** - User-facing extension point

#### IPipelineModuleHooks
- **Location:** `src/ModularPipelines/Interfaces/IPipelineModuleHooks.cs`
- **Visibility:** Public
- **Members:** OnBeforeModuleStartAsync(), OnAfterModuleEndAsync()
- **Purpose:** Module lifecycle hooks
- **Recommendation:** **Keep public** - User-facing extension point

#### IPipelineRequirement
- **Location:** `src/ModularPipelines/Requirements/IPipelineRequirement.cs`
- **Visibility:** Public
- **Members:** MustAsync(), Order
- **Purpose:** Pipeline requirement validation
- **Recommendation:** **Keep public** - User-facing extension point

---

### 4. Validation Interfaces (2)

#### IPipelineValidationService
- **Location:** `src/ModularPipelines/Validation/IPipelineValidationService.cs`
- **Visibility:** Public
- **Purpose:** Orchestrates pipeline validation
- **Recommendation:** **Make internal** - Not intended for external use

#### IPipelineValidator
- **Location:** `src/ModularPipelines/Validation/IPipelineValidator.cs`
- **Visibility:** Public
- **Members:** Order, Validate()
- **Purpose:** Individual validator implementation
- **Recommendation:** **Keep public** - Rename to IValidator for clarity

---

### 5. DI Wrapper (1) - Already Internal

#### IPipelineServiceContainerWrapper
- **Location:** `src/ModularPipelines/DependencyInjection/IPipelineServiceContainerWrapper.cs`
- **Visibility:** Internal
- **Purpose:** DI service collection wrapper
- **Recommendation:** **Keep internal** - Implementation detail

---

### 6. Module Context (1)

#### IModuleContext
- **Location:** `src/ModularPipelines/Context/IModuleContext.cs`
- **Visibility:** Public
- **Inherits From:** IPipelineContext
- **Members:** GetModule<TModule>(), GetModuleIfRegistered<TModule>(), SubModule()
- **Purpose:** Primary interface for module developers
- **Usages:** ~65 references (modules, examples, build)
- **Recommendation:** **Keep public** - Primary user-facing interface

---

## Current Interface Hierarchy

```
IModuleContext
  └── IPipelineContext
        └── IPipelineHookContext
              ├── IPipelineServices
              ├── IPipelineLogging
              ├── IPipelineTools
              ├── IPipelineEncoding
              ├── IPipelineFileSystem
              └── IPipelineEnvironment
```

## Proposed Changes

### 1. Deprecate IPipelineContext
Mark as obsolete with message directing users to IModuleContext.

### 2. Make IPipelineValidationService Internal
Not intended for external consumption.

### 3. Create IValidator Alias
Rename IPipelineValidator to IValidator for clarity, keep old name as obsolete alias.

### 4. Add Context Extension Methods
Create helper extension methods for common operations:
- `GetService<T>()` - simplified DI resolution
- `RunCommand()` - quick command execution
- `ReadFile()` / `WriteFile()` - simplified file operations

---

## Migration Impact

| Change | Breaking? | Migration Path |
|--------|-----------|----------------|
| IPipelineContext obsolete | No | Use IModuleContext instead |
| IPipelineValidationService internal | Potentially | Should not be used externally |
| IPipelineValidator renamed | No | Old name remains as alias |
| Engine interfaces | No | Already internal |

---

## Files Modified in This Audit

This audit covers:
- 23 interface files in `src/ModularPipelines/`
- ~110 usages of IPipelineContext
- ~65 usages of IModuleContext
