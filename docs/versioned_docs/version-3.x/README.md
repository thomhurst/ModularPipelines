---
title: Modular Pipelines
sidebar_position: 0
---

# Modular Pipelines

## About

Modular Pipelines is a framework for writing your pipeline in C#. Giving you strong types, intellisense, parallelisation, and the entire .NET ecosystem at your fingertips.

## Features

*   Parallel execution
*   Dependency management
*   Familiar C# code
*   Ability to debug pipelines
*   Ability to run pipelines locally, even creating versions for setting up local development
*   Strong typing, where different modules/steps can pass data to one another
*   Dependency collision detection - Don't worry about accidentally making two modules dependent on each other
*   Numerous helpers to do things like: Search files, check checksums, (un)zip folders, download files, install files, execute CLI commands, hash data, and more
*   Easy to Skip or Ignore Failures for each individual module by passing in custom logic
*   Hooks that can run before and/or after modules
*   Pipeline requirements - Validate your requirements are met before executing your pipeline, such as a Linux operating system
*   Easy to use File and Folder classes, that can search, read, update, delete and more
*   Source controlled pipelines
*   Build agent agnostic - Can easily move to a different build system without completely recreating your pipeline
*   No need to learn new syntaxes such as YAML defined pipelines
*   Strongly typed wrappers around command line tools
*   Utilise existing .NET libraries
*   Secret obfuscation
*   Grouped logging, and the ability to extend sources by adding to the familiar `ILogger`
*   Run based on categories
*   Easy to read exceptions
*   Dynamic console progress reporting (if the console supports interactive mode)
*   Pretty results table

## Why the name?

### Modular
> consisting of separate parts that, when combined, form a complete whole

### Pipeline
> any channel or means whereby something is passed on

---

### Module
> a self-contained unit or item, such as an assembly of electronic components and associated wiring or a segment of computer software, which itself performs a defined task and can be linked with other such units to form a larger system