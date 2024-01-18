- Log commands standard output and standard error even if they throw exceptions
# Breaking
- CommandException now doesn't expose a CommandResult object. Things like StandardOutput are available directly from the exception