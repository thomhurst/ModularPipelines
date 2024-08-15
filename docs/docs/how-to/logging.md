---
title: Logging
sidebar_position: 8
---

# Logging

## IModuleLogger
When logging in your pipeline, you should always use an IModuleLogger. Do not inject in your own ILogger, and do not write to the console directly using the `Console` class, or any other class that would write directly to the console.

This is because the `IModuleLogger` will perform many actions that will improve the output of your logs.

These are detailed below.

## Secret Obfuscation
If secrets have been defined (See [Secrets](secrets) for details on this), then if any of them are attempted to be written to logs via IModuleLogger, either directly, or through things like HTTP or Command logs, they will be obfuscated in the output. So for instance, if you made a HTTP call with a bearer token of 'MySuperSecretToken', when the HTTP request is logged, that bearer will show as '**********'.

## Grouped Logs
When writing logs through IModuleLogger, logs will be grouped by what module they're in.
Since all modules attempt to run in parallel, if there was no log organisation, then logs would be everywhere and all jumbled up, and hard to navigate. This keeps logs together, clean, and easy to read. This is why it's very important not to write to the console directly, as that'll prevent this from working.

## Interfering with Console Progress 
If you have an interactive terminal, then a progress dialog will be displayed, and constantly updated with the progress of all your modules.
If you start writing to the console directly, then you'll be writing over the top of this progress dialog and messing up how it renders.

## Analyzers
If you forget the above, Modular Pipelines has analyzers built as part of its framework. It'll detect direct uses of `Console`, or trying to inject in custom `ILogger`s and will result in errors, asking you to fix the issues.

## How to access IModuleLogger

### Module
If you're in a module, it's part of your `context` object. Call `context.Logger`.

### Elsewhere
If you're in another class, you can inject in `IModuleLoggerProvider` and call `GetLogger`.