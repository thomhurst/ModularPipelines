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

## MPG0008

A tool accessor cannot generate a discoverable `context.Tools` property because
the consuming project uses a language version older than C# 14. Registration
metadata still generates. Use C# 14 or preview to enable the property.

**Severity:** Warning

## MPG0009

Multiple tool accessors would generate the same discoverable property with
conflicting declarations. Give each accessor a unique name.

**Severity:** Error

## MPG0010

A tool accessor would generate a property whose name is already available on
`IToolsContext` or `object`, such as `Get` or `GetType`. Rename the accessor.

**Severity:** Error

## MPG0011

Module runtime metadata generation was skipped because the module or one of its
containing types is inaccessible to generated code. Make the module and its
containing types accessible before publishing with Native AOT. Runtime reflection
remains available for ordinary JIT deployments.

**Severity:** Warning

## MPG0012

A consumer references a closed generic module declared in another assembly, either
through `AddModule<T>()` or a transitive `DependsOn<T>` chain. The consumer generator
cannot add runtime metadata owned by that external assembly. Use a consumer-owned
non-generic wrapper for the module before publishing with Native AOT.

**Severity:** Warning

## MPG0013

An `AddModule<TModule>()` call uses a type parameter. The source generator cannot
determine every closed construction that may flow through the generic helper, so
trim-safe runtime metadata cannot be generated. Register each concrete module type
directly, or use a non-generic helper that lists concrete registrations explicitly,
before publishing with Native AOT.

**Severity:** Warning

## MPG0014

A module has one or more partial declarations. Another source generator can add a
partial declaration that this generator cannot observe, so dependency metadata
remains incomplete and runtime reflection is used as a fallback. Avoid partial
module declarations before publishing with Native AOT.

**Severity:** Warning

Diagnostics can be configured with standard MSBuild or `.editorconfig` settings.
For example:

```ini
dotnet_diagnostic.MPG0003.severity = error
```
