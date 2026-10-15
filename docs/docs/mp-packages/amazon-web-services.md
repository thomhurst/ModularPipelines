---
title: Amazon Web Services Package
---

# Amazon Web Services Package

Package reserved for strongly typed AWS CLI commands and AWS SDK helpers.

## Installation

```shell
dotnet add package ModularPipelines.AmazonWebServices
```

Required command-line tool: `aws`. It must be installed and available on `PATH` when the pipeline runs.

## API availability

The strongly typed context API is not available yet. This page will include context entry points and examples when the integration is implemented.

## Global options generation

The AWS CLI scraper supports reading the root `GLOBAL OPTIONS` section, including
integer timeouts and enum choices. It preserves command-specific options whose names
also appear in that section and excludes the version action from inherited settings.
These generator capabilities do not make the typed AWS API available in the package;
the generated integration must land separately before callers can use it.

Keep existing AWS CLI invocations until the generated integration is available.
Migration examples for inherited global properties will be added with that integration.
