*   Improved JSON deserialization of modules by including Assembly information within the TypeDescriminator field

# Breaking

*   `context.Dotnet().Test` now returns a `CommandResult`. Previously it would try to log results to a .trx file and parse them out, however different test frameworks use different formats and values for these trx files. So logging to a trx file and parsing it into a strong type model is now left to the user if they wish to do so. The `Test` command will still throw an error (and therefore fail your module) if tests fail.
