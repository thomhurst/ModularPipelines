# ModularPipelines

Define your pipeline in .NET! Strong types, intellisense, parallelisation, and the entire .NET ecosystem at your fingertips.

[![nuget](https://img.shields.io/nuget/v/ModularPipelines.svg)](https://www.nuget.org/packages/ModularPipelines/) 

![Nuget](https://img.shields.io/nuget/dt/ModularPipelines) ![GitHub Workflow Status (with event)](https://img.shields.io/github/actions/workflow/status/thomhurst/ModularPipelines/dotnet.yml) ![GitHub last commit (branch)](https://img.shields.io/github/last-commit/thomhurst/ModularPipelines/main) [![Codacy Badge](https://app.codacy.com/project/badge/Grade/5f14420d97304b42a9e96861a4c0fec4)](https://app.codacy.com/gh/thomhurst/ModularPipelines/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_grade) ![License](https://img.shields.io/github/license/thomhurst/ModularPipelines) [![Codacy Badge](https://app.codacy.com/project/badge/Coverage/5f14420d97304b42a9e96861a4c0fec4)](https://app.codacy.com/gh/thomhurst/ModularPipelines/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_coverage)

## Features

-   Parallel execution with easy waiting on dependencies if necessary
-   Familiar C# code
-   Strong typing, where different modules/steps can pass data to one another
-   Dependency collision detection - Don't worry about accidentally making two modules dependent on each other
-   Numerous helpers to do things like: Search files, check checksums, (un)zip folders, download files, install files, execute CLI commands, hash data, and more
-   Easy to Skip or Ignore Failures for each individual module by passing in custom logic
-   Hooks that can run before and/or after modules
-   Pipeline requirements - Validate your requirements are met before executing your pipeline, such as a Linux operating system
-   Easy to use File and Folder classes, that can search, read, update, delete and more
-   Source controlled pipelines
-   Build agent agnostic - Can easily move to a different build system without completely recreating your pipeline
-   No need to learn new syntaxes such as YAML defined pipelines
-   Strongly typed wrappers around command command line tools
-   Utilise existing .NET libraries
-   Secret obfuscation
-   Grouped logging, and the ability to extend sources by adding to the familiar `ILogger`
-   Dynamic console progress reporting (if the console supports interactive mode)

## Available Modules

%%% AVAILABLE MODULES PLACEHOLDER %%%

## Getting Started

If you want to see how to get started, or want to know more about ModularPipelines, [read the Wiki page here](https://github.com/thomhurst/ModularPipelines/wiki)

## Code Examples

### Program.cs - Main method

```csharp
await PipelineHostBuilder.Create()
    .ConfigureAppConfiguration((context, builder) =>
    {
        builder.AddJsonFile("appsettings.json")
            .AddUserSecrets