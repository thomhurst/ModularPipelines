Why ModularPipelines

- C# / .NET
    - There's less of a learning curve if you're not familiar with different pipeline systems, or scripting, etc. Having it defined in a familiar language makes things easier
    - It's flexible - .NET has lots of powerful features out of the box, and we can tap into the ecosystem of libraries if we want anything more
    - Strong typing
    - Tap into the familiar .NET Host framework, providing full Dependency Injection, flexible Configuration builders 

- Source Control 
    - Some Pipelines, such as TeamCity built ones, aren't source controlled. This means that making changes for a new feature that isn't released yet has to happen globally, which can cause build breakages between different branches. With it in Source Control, we can change the pipeline on a branch for a new feature without affecting other builds and branches
    - A broken pipeline shouldn't ever get merged into the main branch if it never went green
    - We are able to easily look back at a history of changes if they're all stored in git commits
    - We are easily able to identify who made what changes

- Running locally
    - You can debug and run a pipeline locally (targeting test environments ideally), if you ever encounter a broken pipeline
    - How many times have you started a new role or team, and been provided a huge developer setup guide that requires you to download and install numerous different things? You could define a pipeline for local machines. You can decide in startup, depending on something like the Environment name, and decide to either register modules for setting up a local developer machine, or if on a build agent, for deploying to the cloud.  
