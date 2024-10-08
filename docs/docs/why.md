---
sidebar_position: 5
---

# Why Modular Pipelines?

## C# / .NET

- The .NET ecosystem is rich with features and libraries. You can utilize what already exists and not have to re-invent the wheel.

- If you're already a .NET developer, you don't have to concern yourself as much with the different features, languages or syntaxes of different build systems. No need for YAML-based pipeline definitions, Powershell scripts, bash scripts, or even just unfamiliar UI based build systems.

- Strong typing - We can structure our modules with objects that we can pass around, and we know what data we then have available to use.

## Source Control

Some Pipelines, such as TeamCity, aren't typically source controlled. They're built from within the UI itself. This means that making changes for a new feature that isn't released yet has to happen globally, which can cause build breakages between different branches. With it in Source Control, we can change the pipeline on a branch for a new feature without affecting other builds and branches. This makes testing easier and doesn't degrade your production deployments.

A broken pipeline shouldn't ever get merged into the main branch if it never went green. A pipeline change should be made via a pull request, and require a successful build in order to merge.

We are able to easily look back at a history of changes if they're all stored in git commits

We are easily able to identify who made what changes

## Running locally

Many build systems, such as TeamCity and Azure DevOps require running pipelines on their remote agents, and then waiting for a result and parsing through the build output if anything went wrong. It can be very trial-and-error, with lots of lengthy re-runs when trying to debug something.
Because ModularPipelines is not tied to a specific build system, it can be run anywhere that has the .NET SDK installed. That includes your developer machine. So as long as you have adequate permissions, you can debug a pipeline locally on your machine. You get all the benefits of the .NET debugger, so we can view in-memory values, and access exceptions, etc. This makes for a much quicker feedback loop!

Because a pipeline can be run locally, there's nothing to stop you from creating a 'local developer pipeline'.
How many times have you started a new role or team, and been provided a huge developer setup guide that requires you to download and install numerous different things? You could define a pipeline for local machines. You can decide in startup, depending on something like the Environment name, and decide to either register modules for setting up a local developer machine, or if on a build agent, for deploying to the cloud.

## Portability

Want to move to a different build system? You don't have to re-learn or setup the whole thing from scratch. Your system simply needs access to your Pipeline project and have the .NET SDK installed.