---
title: Source generator diagnostics
sidebar_position: 18
---

# Source generator diagnostics

Modular Pipelines generates registration metadata at compile time. A generator
diagnostic explains why metadata could not be emitted safely and whether the build
will fail or runtime reflection will be used as a fallback.

## MPG0001

An invalid method has `[ModularPipelinesIntegration]`. The method and its containing
type must be accessible and non-generic. The method must be static, accept one
`IServiceCollection` by-value parameter, and return either `void` or
`IServiceCollection`.

**Severity:** Error

## MPG0002

Two module types produce the same generated accessor name. Rename one module so
stripping the `Module` suffix produces a unique name.

**Severity:** Error

## MPG0003

An inaccessible property with a CLI attribute prevents complete command metadata
generation. Make the property and getter accessible to generated code. Until fixed,
Modular Pipelines uses runtime reflection for the missing metadata.

**Severity:** Warning

## MPG0004

An inaccessible property marked with `[SecretValue]` prevents complete secret
metadata generation. Make the property and getter accessible to generated code.
Until fixed, Modular Pipelines uses runtime reflection for the missing metadata.

**Severity:** Warning

## MPG0005

A module attribute cannot be constructed by generated code. Attribute types,
constructors, named members, and referenced argument types must be accessible.
Attribute arguments must also be representable as C# literals. Until fixed,
Modular Pipelines uses runtime reflection for the missing metadata.

**Severity:** Warning

## MPG0006

Command or secret metadata generation was skipped because its declaring type is
generic or inaccessible to generated code. Make the type and its containing types
accessible and non-generic. Until fixed, Modular Pipelines uses runtime reflection.

**Severity:** Info

## MPG0007

Module event metadata generation was skipped because a concrete module type is
generic or inaccessible to generated code. Make the type and its containing types
accessible and non-generic. Until fixed, Modular Pipelines uses runtime reflection.

**Severity:** Info

Diagnostics can be configured with standard MSBuild or `.editorconfig` settings.
For example:

```ini
dotnet_diagnostic.MPG0003.severity = error
```
