module Fs.Obsidian.Commands

open System.Text.RegularExpressions
open Browser
open Browser.Types
open Fable.Core
open ObsidianBindings
open Fable.Core.JsInterop
open Fable.Import
open Fable.Core.Extensions

[<Literal>]
let headingregex = """^(#{1,6}) """

let doNone = (fun f -> None)

let rec goToPrevHeading (plugin: ExtendedPlugin<PluginSettings>) =
    Command.forEditor
        (nameof goToPrevHeading)
        "Go to previous heading"
        (fun editor ->
            let cursor = editor.getCursor ()

            match
                plugin.app
                |> Content.getHeadings
            with
            | None ->
                Notice.show "no headings found"
                ()
            | Some headings ->
                let targetHeading =
                    headings
                    |> Seq.rev
                    |> Seq.tryFind (fun v -> v.startLine < (int cursor.line))

                match targetHeading with
                | None -> ()
                | Some heading ->
                    let headingLine = heading.startLine
                    editor.setCursor (U2.Case2(float (headingLine - 1)))

            doNone
        )



let rec goToNextHeading (plugin: ExtendedPlugin<PluginSettings>) =
    Command.forEditor
        (nameof goToNextHeading)
        "Go to next heading"
        (fun editor ->
            let cursor = editor.getCursor ()

            match
                plugin.app
                |> Content.getHeadings
            with
            | None ->
                Notice.show "no headings found"
                ()
            | Some headings ->
                let targetHeading =
                    headings
                    |> Seq.tryFind (fun v -> v.startLine > (int cursor.line + 1))

                match targetHeading with
                | None -> ()
                | Some heading ->
                    let headingLine = heading.startLine
                    editor.setCursor (U2.Case2(float (headingLine - 1)))

            doNone
        )


let rec selectCurrentBlock (plugin: ExtendedPlugin<PluginSettings>) =
    Command.forEditor
        (nameof selectCurrentBlock)
        "Select current heading block"
        (fun editor ->
            let cursor = editor.getCursor ()

            match
                plugin.app
                |> Content.getHeadings
            with
            | None ->
                Notice.show "no headings found"
                ()
            | Some headings ->
                let topHeadingOpt =
                    headings
                    |> Seq.rev
                    |> Seq.tryFind (fun v -> v.startLine <= (int cursor.line + 1))

                topHeadingOpt
                |> Option.iter (fun topHeading ->
                    headings
                    |> Seq.tryFind (fun v ->
                        v.startLine > (int topHeading.startLine)
                    )
                    |> Option.map (fun bottomHeading ->
                        let headingLine = topHeading.startLine
                        let endlineText = editor.getLine(float bottomHeading.startLine - 2.)
                        editor.setSelection(
                            EditorPosition.create 0 (float topHeading.startLine - 1.),
                            EditorPosition.create (float endlineText.Length ) (float bottomHeading.startLine - 2.)
                        )
                    )
                    |> Option.defaultWith (fun _ ->
                        // no bottom heading
                        editor.setSelection(
                            EditorPosition.create 0 (float topHeading.startLine - 1.),
                            EditorPosition.create (0.) (editor.lastLine())
                        )
                    )
                )

            doNone
        )


let rec goToPrevEmptyLine (plugin: ExtendedPlugin<PluginSettings>) =
    Command.forEditor
        (nameof goToPrevEmptyLine)
        "Go to previous empty line"
        (fun editor ->
            let cursor = editor.getCursor ()

            let linesbefore =
                editor.getValue().Split('\n').[.. int cursor.line]
                |> Array.rev

            let currentIsEmpty = Regex.Match(editor.getLine (int cursor.line), "^\\s*$").Success

            let foundOpt =
                if not currentIsEmpty then
                    linesbefore
                    |> Seq.skipSafe 1
                    |> Seq.tryFindIndex (fun f -> Regex.Match(f, "^\\s*$").Success)
                else
                    let nToSkip =
                        linesbefore
                        |> Seq.tryFindIndex (fun f -> not (Regex.Match(f, "^\\s*$").Success))
                        |> Option.map ((+) 1)
                        |> Option.defaultValue 1

                    linesbefore
                    |> Seq.skipSafe nToSkip
                    |> Seq.tryFindIndex (fun f -> Regex.Match(f, "^\\s*$").Success)
                    |> Option.map (
                        (+) (
                            nToSkip
                            - 1
                        )
                    )

            match foundOpt with
            | None -> doNone
            | Some moveby ->
                let newpos =
                    int cursor.line
                    - moveby
                    - 1
                    |> float

                editor.setCursor (U2.Case2 newpos)
                doNone
        )


let rec goToNextEmptyLine (plugin: ExtendedPlugin<PluginSettings>) =
    Command.forEditor
        (nameof goToNextEmptyLine)
        "Go to next empty line"
        (fun editor ->
            let cursor = editor.getCursor ()

            let linesafter = editor.getValue().Split('\n').[int cursor.line ..]

            let currentIsEmpty = Regex.Match(editor.getLine (int cursor.line), "^\\s*$").Success

            let foundOpt =
                if not currentIsEmpty then
                    linesafter
                    |> Seq.skipSafe 1
                    |> Seq.tryFindIndex (fun f -> Regex.Match(f, "^\\s*$").Success)
                else
                    let nToSkip =
                        linesafter
                        |> Seq.tryFindIndex (fun f -> not (Regex.Match(f, "^\\s*$").Success))
                        |> Option.map ((+) 1)
                        |> Option.defaultValue 1

                    linesafter
                    |> Seq.skipSafe nToSkip
                    |> Seq.tryFindIndex (fun f -> Regex.Match(f, "^\\s*$").Success)
                    |> Option.map (
                        (+) (
                            nToSkip
                            - 1
                        )
                    )

            match foundOpt with
            | None -> doNone
            | Some moveby ->
                let newpos =
                    int cursor.line
                    + moveby
                    + 1
                    |> float

                editor.setCursor (U2.Case2 newpos)
                doNone
        )


let rec copyNextCodeBlock (plugin: ExtendedPlugin<PluginSettings>) =
    Command.forEditor
        (nameof copyNextCodeBlock)
        "Copy Next Code Block"
        (fun edit ->
            match
                plugin.app
                |> Content.getCodeBlocks
            with
            | None -> doNone
            | Some blocks ->
                let cursor = edit.getCursor ()

                blocks
                |> Seq.tryFind (fun f -> f.startLine > (int cursor.line))
                |> function
                    | None ->
                        obsidian.Notice.Create(U2.Case1 "could not find a code block")
                        |> ignore
                    | Some v ->
                        $"copied:\n{v.content.Substring(0, min (v.content.Length) 50)}"
                        |> U2.Case1
                        |> obsidian.Notice.Create
                        |> ignore

                        Clipboard.write v.content
                        |> ignore


                doNone
        )

let rec copyCodeBlock (plugin: ExtendedPlugin<PluginSettings>) =
    Command.forMenu
        (nameof copyCodeBlock)
        "Copy Code Block"
        (fun _ ->
            let codeblocks =
                plugin.app
                |> Content.getCodeBlocks

            if codeblocks.IsNone then
                None
            else

                let codeblocks = codeblocks.Value

                let modal =
                    plugin.app
                    |> SuggestModal.create
                    |> SuggestModal.withGetSuggestions (fun queryInput ->
                        let query = obsidian.prepareQuery queryInput

                        let matches =
                            codeblocks
                            |> Seq.map (fun f ->
                                let text = f.content
                                f, obsidian.fuzzySearch (query, text)
                            )
                            |> Seq.where (fun f ->
                                snd f
                                |> Option.isSome
                            )
                            |> Seq.map fst

                        matches
                        |> ResizeArray
                    )
                    |> SuggestModal.withRenderSuggestion (fun f elem ->
                        elem.innerText <- f.content
                    )
                    |> SuggestModal.withOnChooseSuggestion (fun (f, args) ->
                        $"copied:\n{f.content.Substring(0, min (f.content.Length) 50)}"
                        |> U2.Case1
                        |> obsidian.Notice.Create
                        |> ignore

                        Clipboard.write f.content
                        |> ignore
                    )

                modal.``open`` ()
                None
        )

let rec openSwitcherWithTag1 (plugin: ExtendedPlugin<PluginSettings>) =
    Command.forMenu
        (nameof openSwitcherWithTag1)
        "Open Switcher with Tag 1"
        (fun _ ->
            match
                plugin.app
                |> Content.getTags
            with
            | None -> Notice.show "no tags found"
            | Some tags when tags.Count = 0 -> Notice.show "no tags found"
            | Some tags ->
                let cmd = plugin.settings.defaultModalCommand

                match plugin.app?commands?executeCommandById (cmd) with
                | false ->
                    Notice.show
                        $"failed to run command: {cmd}, configure Default modal command in settings"
                | true ->
                    match document.querySelector ("input.prompt-input") with
                    | null -> Notice.show "plugin outdated"
                    | modalInput ->
                        modalInput?value <- $"%s{tags[0].tag} "
                        let ev = Browser.Event.Event.Create("input", null)

                        modalInput.dispatchEvent (ev)
                        |> ignore

            None
        )

let rec tagSearch (plugin: ExtendedPlugin<PluginSettings>) =
    Command.forMenu
        (nameof tagSearch)
        "Search by Tag"
        (fun _ ->

            let getVaultTags () =
                plugin.app.vault.getMarkdownFiles ()
                |> Seq.collect plugin.app.getTagsOfFile
                |> Seq.groupBy id
                |> Seq.map (fun (tag, tags) ->
                    tag,
                    tags
                    |> Seq.length
                )

            plugin.app
            |> SuggestModal.create
            |> SuggestModal.withGetSuggestions (fun queryInput ->
                let query = obsidian.prepareQuery queryInput

                let matches =
                    getVaultTags ()
                    |> Seq.map (fun (tag, count) -> {| count = count; tag = tag |})
                    |> Seq.choose (fun f ->
                        obsidian.fuzzySearch (query, f.tag)
                        |> Option.map (fun search -> f, search.score)
                    )
                    |> (fun results ->
                        match queryInput with
                        | "" ->
                            results
                            |> Seq.sortByDescending (fun f -> (fst f).count)
                        | _ ->
                            results
                            |> Seq.sortByDescending snd
                    )
                    |> Seq.map fst

                matches
                |> ResizeArray
            )
            |> SuggestModal.withRenderSuggestion (fun f elem ->
                elem.innerText <- $"{f.count}:\t{f.tag}"
            )
            |> SuggestModal.withOnChooseSuggestion (fun (chosenResult, eventArgs) ->
                let cmd = plugin.settings.defaultModalCommand

                match plugin.app?commands?executeCommandById (cmd) with
                | false ->
                    Notice.show
                        $"failed to run command: {cmd}, configure Default modal command in settings"
                | true ->
                    match document.querySelector ("input.prompt-input") with
                    | null -> Notice.show "plugin outdated"
                    | modalInput ->
                        modalInput?value <- $"{chosenResult.tag} "
                        let ev = Browser.Event.Event.Create("input", null)

                        modalInput.dispatchEvent (ev)
                        |> ignore
            )
            |> SuggestModal.openModal

            None
        )


type FoldedTagSearchAction =
    | ExpandTag
    | AddAnotherTag

type FoldedTagSearchState = {
    Level: int
    Query: string
    Filters: string list
    Actions: FoldedTagSearchAction list
} with

    static member Default: FoldedTagSearchState = {
        Level = 1
        Query = ""
        Filters = []
        Actions = []
    }

let rec foldedTagSearch (plugin: ExtendedPlugin<PluginSettings>) =
    Command.forMenu
        (nameof foldedTagSearch)
        "Folded search by Tag"
        (fun _ ->

            let startState = FoldedTagSearchState.Default

            let getVaultTags (state: FoldedTagSearchState) =
                plugin.app.vault.getMarkdownFiles ()
                |> Seq.map (
                    plugin.app.getTagsOfFile
                    >> Seq.toArray
                )
                |> Seq.where (fun tags ->
                    state.Filters
                    |> List.forall (fun filter ->
                        tags
                        |> Array.exists (fun f -> f.StartsWith filter)
                    )
                    && tags
                       |> Array.exists (fun f -> f.StartsWith state.Query)
                )
                |> Seq.collect (fun tags ->
                    tags
                    |> Seq.where (fun f -> f.StartsWith state.Query)
                )
                |> Seq.groupBy (fun f ->
                    f
                    |> String.untilNthOccurrence state.Level '/'
                )
                |> Seq.map (fun (tag, tags) ->
                    tag,
                    tags
                    |> Seq.length
                )


            let rec createModal (state: FoldedTagSearchState) =

                let undoLastAction =
                    (fun (evt, (modal: SuggestModal<{| count: int; tag: string |}>)) ->
                        match state.Actions with
                        | [] -> () // no actions taken
                        | AddAnotherTag :: tail ->
                            match state.Filters with
                            | [] -> () // no filters
                            | _ ->
                                modal.close ()

                                {
                                    FoldedTagSearchState.Default with
                                        Filters =
                                            state.Filters
                                            |> List.tail
                                        Actions =
                                            tail
                                            |> List.skipWhile (fun f ->
                                                f
                                                <> AddAnotherTag
                                            )
                                }
                                |> createModal
                        | ExpandTag :: tail ->
                            modal.close ()
                            let currSelection = modal.currentSelection

                            let prevLevel =
                                state.Level
                                - 1

                            let newQuery =
                                match prevLevel with
                                | 1 -> ""
                                | _ ->
                                    currSelection.tag
                                    |> String.untilNthOccurrence
                                        (prevLevel
                                         - 1)
                                        '/'

                            {
                                state with
                                    Level = prevLevel
                                    Query = newQuery
                                    Actions = tail
                            }
                            |> createModal
                    )


                plugin.app
                |> SuggestModal.create
                |> SuggestModal.map (fun sm ->
                    state.Query
                    :: state.Filters
                    |> String.concat " AND "
                    |> sm.setPlaceholder
                )
                |> SuggestModal.withInstructions [
                    "Tab", "Expand tag"
                    "Ctrl+Enter", "Add another tag"
                    "Shift+Enter/Shift+Tab", "Undo last action"
                    "Enter", "Search"
                ]
                |> SuggestModal.withGetSuggestions2 (fun queryInput ->
                    let query = obsidian.prepareQuery queryInput

                    let matches =
                        getVaultTags state
                        |> Seq.map (fun (tag, count) -> {| count = count; tag = tag |})
                        |> Seq.choose (fun f ->
                            obsidian.fuzzySearch (query, f.tag)
                            |> Option.map (fun search -> f, search.score)
                        )
                        |> (fun results ->
                            match queryInput with
                            | "" ->
                                results
                                |> Seq.sortByDescending (fun f -> (fst f).count)
                            | _ ->
                                results
                                |> Seq.sortByDescending snd
                        )
                        |> Seq.map fst

                    matches
                )
                |> SuggestModal.withRenderSuggestion (fun f elem ->
                    elem.innerText <- $"{f.count}:\t{f.tag}"
                )
                |> SuggestModal.withKeyboardShortcut {
                    modifiers = [ Modifier.Shift ] // Undo action 1
                    key = "Enter"
                    action = undoLastAction
                }
                |> SuggestModal.withKeyboardShortcut {
                    modifiers = [ Modifier.Shift ] // Undo action 2
                    key = "Tab"
                    action = undoLastAction
                }
                |> SuggestModal.withCtrlKeyboardShortcut {
                    modifiers = []
                    key = "Enter"
                    action =
                        (fun (evt, modal) ->
                            let current = modal.currentSelection.tag

                            let exists =
                                state.Filters
                                |> Seq.exists (fun f -> f = current)

                            match exists with
                            | true -> () // filter already exists
                            | false ->
                                modal.close ()

                                {
                                    FoldedTagSearchState.Default with
                                        Filters =
                                            modal.currentSelection.tag
                                            :: state.Filters
                                        Actions =
                                            AddAnotherTag
                                            :: state.Actions
                                }
                                |> createModal
                        )
                }
                |> SuggestModal.withKeyboardShortcut {
                    modifiers = []
                    key = "Tab"
                    action =
                        (fun (evt, modal) ->
                            let currSelection = modal.currentSelection
                            // if no slash then there are no children to expand
                            match modal.currentSelection.tag.EndsWith "/" with
                            | false -> ()
                            | true ->
                                let newState = {
                                    state with
                                        Level =
                                            state.Level
                                            + 1
                                        Query = currSelection.tag
                                        Actions =
                                            ExpandTag
                                            :: state.Actions
                                }

                                modal.close ()
                                createModal newState
                        )
                }
                |> SuggestModal.withOnChooseSuggestion (fun (chosenResult, eventArgs) ->
                    let cmd = plugin.settings.defaultModalCommand

                    match plugin.app.executeCommandById (cmd) with
                    | false ->
                        Notice.show
                            $"failed to run command: {cmd}, configure Default modal command in settings"
                    | true ->
                        match document.querySelector ("input.prompt-input") with
                        | null -> Notice.show "plugin outdated"
                        | modalInput ->
                            let searchString =
                                chosenResult.tag
                                :: state.Filters
                                |> String.concat " "
                                |> (fun f -> f + " ")

                            modalInput?value <- searchString
                            let ev = Browser.Event.Event.Create("input", null)

                            modalInput.dispatchEvent (ev)
                            |> ignore
                )
                |> SuggestModal.openModal

            createModal startState

            None
        )

let rec insertHeading4 (plugin: ExtendedPlugin<PluginSettings>) =
    Command.forEditor
        (nameof insertHeading4)
        "Insert heading 4"
        (fun edit ->
            edit.replaceSelection ("#### ")
            doNone
        )

let rec insertHeading5 (plugin: ExtendedPlugin<PluginSettings>) =
    Command.forEditor
        (nameof insertHeading5)
        "Insert heading 5"
        (fun edit ->
            edit.replaceSelection ("##### ")
            doNone
        )

let rec increaseHeading (plugin: ExtendedPlugin<PluginSettings>) =
    Command.forEditor
        (nameof increaseHeading)
        "Increase Heading level"
        (fun edit ->
            let lineIdx = edit.getCursor().line

            let currLine =
                lineIdx
                |> edit.getLine

            match currLine.StartsWith("#"), currLine.StartsWith("######") with
            | false, _
            | _, true -> doNone
            | _ ->
                edit.setLine (lineIdx, $"#{currLine}")
                doNone
        )

let rec decreaseHeading (plugin: ExtendedPlugin<PluginSettings>) =
    Command.forEditor
        (nameof decreaseHeading)
        "Decrease Heading level"
        (fun edit ->
            let lineIdx = edit.getCursor().line

            let currLine =
                lineIdx
                |> edit.getLine

            match currLine.StartsWith("##") with
            | false -> doNone
            | _ ->
                edit.setLine (lineIdx, currLine[1..])
                doNone
        )

let rec insertDefaultCallout (plugin: ExtendedPlugin<PluginSettings>) =
    Command.forEditor
        (nameof insertDefaultCallout)
        "Insert Default Callout"
        (fun edit ->
            edit.replaceSelection ($"> [!{plugin.settings.defaultCalloutType}] ")
            doNone
        )

let rec insertCodeBlock (plugin: ExtendedPlugin<PluginSettings>) =
    Command.forEditor
        (nameof insertCodeBlock)
        "Insert Code Block"
        (fun edit ->
            let newBlock =
                match plugin.settings.use3BackticksForCodeBlock with
                | true -> ($"```{plugin.settings.defaultCodeBlockLanguage}\n\n```")
                | false -> ($"````{plugin.settings.defaultCodeBlockLanguage}\n\n````")

            edit.replaceSelection newBlock
            let cursor = edit.getCursor ()

            edit.setCursor (
                U2.Case2(
                    cursor.line
                    - 1.
                )
            )

            doNone
        )

let rec insertTest (plugin: ExtendedPlugin<PluginSettings>) =

    Command.forEditor
        "debug-test"
        "debug-test"
        (fun edit ->
            let tags = plugin.app.getAllTags ()
            let keys = JS.Constructors.Object.keys (tags)

            let currFile = plugin.app.workspace.getActiveFile().Value

            let tags2 =
                plugin.app.getTagsOfFile (currFile)
                |> Seq.toArray

            let fm1 =
                plugin.app.vault.getMarkdownFiles ()
                |> Seq.choose plugin.app.metadataCache.getFileCache
                |> Seq.toArray

            // JS.Constructors.Object.
            // let value : int = tags?(keys[0])
            // console.log value

            console.log fm1
            console.log tags2

            doNone
        )
