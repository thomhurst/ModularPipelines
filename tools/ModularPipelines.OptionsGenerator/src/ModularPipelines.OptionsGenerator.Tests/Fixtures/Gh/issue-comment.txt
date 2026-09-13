Add a comment to a GitHub issue.

Without body text or attachments supplied through flags, the command will
interactively prompt for the comment text.

Use `--attach` to upload an image or video. If the body already references an
attached file, such as `![alt](./login.png)`, that reference is rewritten to point
at the uploaded asset. Any attached file the body does not reference is appended
to the end of the comment.
You can attach up to 50 files per command.

Alt text for an image follows the path after `#`, as in
`--attach './login.png#The login error state'`. Without it the filename is used.
A reference already in the body keeps the alt text written there. Video renders
as a player and has no alt text, so it cannot be given any.


USAGE
  gh issue comment {<number> | <url>} [flags]

FLAGS
      --attach file      Attach an image or video file, in '<file>#<image alt text>' format
  -b, --body text        The comment body text
  -F, --body-file file   Read body text from file (use "-" to read from standard input)
      --create-if-none   Create a new comment if no comments are found. Can be used only with --edit-last
      --delete-last      Delete the last comment of the current user
      --edit-last        Edit the last comment of the current user
  -e, --editor           Skip prompts and open the text editor to write the body in
  -w, --web              Open the web browser to write the comment
      --yes              Skip the delete confirmation prompt when --delete-last is provided

INHERITED FLAGS
      --help                     Show help for command
  -R, --repo [HOST/]OWNER/REPO   Select another repository using the [HOST/]OWNER/REPO format

EXAMPLES
  # Add a comment to an issue
  $ gh issue comment 12 --body "Hi from GitHub CLI"

  # Attach a screenshot, with alt text after "#"
  $ gh issue comment 12 --attach './login.png#The login error state'

  # Attach multiple files by repeating the flag
  $ gh issue comment 12 --attach ./before.png --attach ./after.png

LEARN MORE
  Use `gh <command> <subcommand> --help` for more information about a command.
  Read the manual at https://cli.github.com/manual
  Learn about exit codes using `gh help exit-codes`
  Learn about accessibility experiences using `gh help accessibility`
