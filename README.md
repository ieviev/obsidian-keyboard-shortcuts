# Quick snippets and navigation for Obsidian

- Keyboard navigation up/down for headings
- Configurable default code block and callout shortcuts
- Copy code block via keyboard shortcut
- Quick Switcher extensions

no keybinds are set on install, you have to configure them yourself

## Go to previous/next heading

- note: only works in edit mode


![](https://github.com/aciq/obsidian-keyboard-shortcuts/blob/main/_resources/obs-go-to-heading.gif?raw=true)

## Copy code block / Copy next code block 

- Copy code block: shows a list of code blocks and allows you to search and copy one of them
- Copy next code block: copies the next code block, closest to the cursor

![](https://github.com/aciq/obsidian-keyboard-shortcuts/blob/main/_resources/obs-copying-codeblocks.gif?raw=true)

## Switcher extensions

NOTE: these require you to configure `Default modal command` in the plugin settings:

For the following demos i used the [Another Quick Switcher plugin](https://github.com/tadashi-aikawa/obsidian-another-quick-switcher) which supports tags and set `Default modal command` to:

```
obsidian-another-quick-switcher:search-command_Recent search
```


### Switcher extensions / Search by tag

Allows performing a multi-step search, first filtering notes by tag.

![](https://github.com/aciq/obsidian-keyboard-shortcuts/blob/main/_resources/obs-search-by-tag.gif?raw=true)

### Switcher extensions / Open switcher with Tag 1

Opens the switcher with the first tag in the file already added to the query.

![](https://github.com/aciq/obsidian-keyboard-shortcuts/blob/main/_resources/obs-with-tag-1.gif?raw=true)



## Insert heading 4/5 / Insert code block / Insert callout

- also moves the cursor inside the code block / to the title after using
- the default language of the code block is configurable in settings
- the default callout type is also configurable in settings

![](https://github.com/aciq/obsidian-keyboard-shortcuts/blob/main/_resources/obs-inserting-codeblocks-headings.gif?raw=true)


# development

## dependencies

- dotnet 6.0 sdk
- npm

## ---

installing node and dotnet dependencies
```
npm i
npm run install
```

compiling F# to js
```
npm run build
```

publishing to dist/
```
npm run publish
```