Create an issue on GitHub.

Use `--attach` to upload an image or video. The attachment is appended to the
body. If the body references an attached file, such as `![alt](./login.png)`, that
reference is rewritten to point at the uploaded asset instead.
You can attach up to 50 files per command.

Alt text for an image follows the path after `#`, as in
`--attach './login.png#The login error state'`. Without it the filename is used.
A reference already in the body keeps the alt text written there. Video renders
as a player and has no alt text, so it cannot be given any.

If some attachments upload and others fail, the issue is still created with the
ones that succeeded. The command then exits with a non-zero status, but the new
issue's URL is still printed to stdout.

Adding an issue to projects requires authorization with the `project` scope.
To authorize, run `gh auth refresh -s project`.

The `--assignee` flag supports the following special values:
- `@me`: assign yourself
- `@copilot`: assign Copilot (not supported on GitHub Enterprise Server)


USAGE
  gh issue create [flags]

ALIASES
  gh issue new

FLAGS
  -a, --assignee login       Assign people by their login. Use "@me" to self-assign.
      --attach file          Attach an image or video file, in '<file>#<image alt text>' format
      --blocked-by numbers   Mark the new issue as blocked by these issue numbers or URLs
      --blocking numbers     Mark the new issue as blocking these issue numbers or URLs
  -b, --body string          Supply a body. Will prompt for one otherwise.
  -F, --body-file file       Read body text from file (use "-" to read from standard input)
  -e, --editor               Skip prompts and open the text editor to write the title and body in. The first line is the title and the remaining text is the body.
  -l, --label name           Add labels by name
  -m, --milestone name       Add the issue to a milestone by name
      --parent number        Add the new issue as a sub-issue of the specified parent number or URL
  -p, --project title        Add the issue to projects by title
      --recover string       Recover input from a failed run of create
  -T, --template name        Template name to use as starting body text
  -t, --title string         Supply a title. Will prompt for one otherwise.
      --type name            Set the issue type by name
  -w, --web                  Open the browser to create an issue

INHERITED FLAGS
      --help                     Show help for command
  -R, --repo [HOST/]OWNER/REPO   Select another repository using the [HOST/]OWNER/REPO format

EXAMPLES
  $ gh issue create --title "I found a bug" --body "Nothing works"
  $ gh issue create --attach './login.png#The login error state'
  $ gh issue create --attach ./before.png --attach ./after.png
  $ gh issue create --label "bug,help wanted"
  $ gh issue create --label bug --label "help wanted"
  $ gh issue create --assignee monalisa,hubot
  $ gh issue create --assignee "@me"
  $ gh issue create --assignee "@copilot"
  $ gh issue create --project "Roadmap"
  $ gh issue create --template "Bug Report"
  $ gh issue create --type Bug
  $ gh issue create --parent 100
  $ gh issue create --parent https://github.com/cli/go-gh/issues/42
  $ gh issue create --blocked-by 200,201 --blocking 300

LEARN MORE
  Use `gh <command> <subcommand> --help` for more information about a command.
  Read the manual at https://cli.github.com/manual
  Learn about exit codes using `gh help exit-codes`
  Learn about accessibility experiences using `gh help accessibility`
