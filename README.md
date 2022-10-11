# Quick snippets and navigation for Obsidian

- Keyboard navigation up/down for headings
- Configurable default code block and callout shortcuts
- Copy code block via keyboard shortcut

no keybinds are set on install, you have to configure them yourself

## Go to previous/next heading

- note: only works in edit mode


![](https://github.com/aciq/obsidian-keyboard-shortcuts/blob/main/_resources/obs-go-to-heading.gif?raw=true)

## Copy code block / Copy next code block 

- Copy code block: shows a list of code blocks and allows you to search and copy one of them
- Copy next code block: copies the next code block, closest to the cursor

![](https://github.com/aciq/obsidian-keyboard-shortcuts/blob/main/_resources/obs-copying-codeblocks.gif?raw=true)


## Insert heading 4 / Insert code block / Insert callout

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