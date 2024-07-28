---
title: Analyzers
---

# Analyzers

## Built-in
A few Analyzers have been built and come out-of-the-box when using Modular Pipelines. This is to prevent errors at runtime, and bring them to the developer's attention at compile time.

These include:
- Checks for injecting ILogger
- `Console` class usage
- Missing `DependsOn` attributes
- Conflicting `DependsOn` attributes
- Returning `IEnumerable<T>`