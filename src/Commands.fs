module Fs.Obsidian.Commands

open System.Text.RegularExpressions
open Browser
open Browser.Types
open Fable.Core
open ObsidianBindings
open Fable.Core.JsInterop
open Fable.Import
open Fable.Core.Extensions

let [<Literal>] headingregex = """^(#{1,6}) """
let doNone = (fun f -> None)

let rec goToPrevHeading (plugin:ExtendedPlugin<PluginSettings>) =
    Command.forEditor (nameof goToPrevHeading) "Go to previous heading"
        (fun editor ->
            let cursor = editor.getCursor()
            let linesbefore =
                editor.getValue().Split('\n').[..int cursor.line] |> Array.rev
            
            let foundOpt = 
                linesbefore
                |> Seq.skipSafe 1
                |> Seq.tryFindIndex(fun f ->
                    Regex.Match(f,headingregex).Success )
            
            match foundOpt with
            | None -> doNone
            | Some moveby ->
                let newpos = int cursor.line - moveby - 1 |> float
                editor.setCursor(U2.Case2 newpos)
                doNone
        )
        
let rec goToNextHeading (plugin:ExtendedPlugin<PluginSettings>) =
    Command.forEditor (nameof goToNextHeading) "Go to next heading"
        (fun editor ->
            let cursor = editor.getCursor()
            let linesafter = editor.getValue().Split('\n').[int cursor.line..]
            
            let foundOpt = 
                linesafter
                |> Seq.skipSafe 1
                |> Seq.tryFindIndex(fun f ->
                    Regex.Match(f,headingregex).Success )
            
            match foundOpt with
            | None -> doNone
            | Some moveby ->
                let newpos = int cursor.line + moveby + 1 |> float
                editor.setCursor(U2.Case2 newpos)
                doNone
        )

let rec copyNextCodeBlock (plugin:ExtendedPlugin<PluginSettings>) =
    Command.forEditor (nameof copyNextCodeBlock) "Copy Next Code Block"
        (fun edit ->
            match plugin.app |> Content.getCodeBlocks with
            | None -> doNone
            | Some blocks ->  
                let cursor = edit.getCursor()
                blocks
                |> Seq.tryFind (fun f -> f.startLine > (int cursor.line))
                |> function
                    | None -> obsidian.Notice.Create(U2.Case1 "could not find a code block") |> ignore 
                    | Some v -> 
                        $"copied:\n{v.content.Substring(0, min (v.content.Length) 50)}"
                        |> U2.Case1 |> obsidian.Notice.Create |> ignore
                        Clipboard.write v.content |> ignore
                       
                        
                doNone
        )

let rec copyCodeBlock (plugin:ExtendedPlugin<PluginSettings>) =
    Command.forMenu (nameof copyCodeBlock) "Copy Code Block"
        (fun _ ->
            let codeblocks = plugin.app |> Content.getCodeBlocks
            if codeblocks.IsNone then None else
                
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
                            f, obsidian.fuzzySearch(query,text)
                        )
                        |> Seq.where (fun f -> snd f |> Option.isSome)
                        |> Seq.map fst
                    matches |> ResizeArray
                    )
                |> SuggestModal.withRenderSuggestion (fun f elem ->
                    elem.innerText <- f.content)
                |> SuggestModal.withOnChooseSuggestion (fun (f,args) ->
                    $"copied:\n{f.content.Substring(0, min (f.content.Length) 50)}"
                    |> U2.Case1 |> obsidian.Notice.Create |> ignore
                    Clipboard.write f.content |> ignore
                )
                
            modal.``open``()
            None
        )
    
let rec openSwitcherWithTag1 (plugin:ExtendedPlugin<PluginSettings>) =
    Command.forMenu (nameof openSwitcherWithTag1) "Open Switcher with Tag 1"
        (fun _ ->
            match plugin.app |> Content.getTags with 
            | None -> Notice.show "no tags found"
            | Some tags when tags.Count = 0 -> Notice.show "no tags found"
            | Some tags -> 
                let cmd = plugin.settings.defaultModalCommand
                match plugin.app?commands?executeCommandById(cmd) with 
                | false -> 
                    Notice.show $"failed to run command: {cmd}, configure Default modal command in settings"
                | true -> 
                    match document.querySelector("input.prompt-input")  with 
                    | null -> Notice.show "plugin outdated"
                    | modalInput -> 
                        modalInput?value <- $"%s{tags[0].tag} "
                        let ev = Browser.Event.Event.Create("input",null)
                        modalInput.dispatchEvent(ev) |> ignore
            None
        )    

let rec tagSearch (plugin:ExtendedPlugin<PluginSettings>) =
    Command.forMenu (nameof tagSearch) "Search by Tag"
        (fun _ ->

            let getVaultTags() = 
                plugin.app.vault.getMarkdownFiles()
                |> Seq.choose plugin.app.metadataCache.getFileCache
                |> Seq.choose (fun f -> f.tags)
                |> Seq.collect id
                |> Seq.groupBy (fun f -> f.tag)
                |> Seq.map (fun (tag,tags) -> tag, tags |> Seq.length)

            plugin.app
            |> SuggestModal.create
            |> SuggestModal.withGetSuggestions (fun queryInput ->
                let query = obsidian.prepareQuery queryInput
                let matches =
                    getVaultTags()
                    |> Seq.map (fun (tag,count) -> {|count=count;tag=tag|} )
                    |> Seq.choose (fun f -> obsidian.fuzzySearch(query,f.tag) |> Option.map (fun search -> f,search.score) )
                    |> (fun results -> 
                        match queryInput with 
                        | "" -> results |> Seq.sortByDescending (fun f -> (fst f).count)
                        | _ -> results |> Seq.sortByDescending snd
                    )
                    |> Seq.map fst
                matches |> ResizeArray
                )
            |> SuggestModal.withRenderSuggestion (fun f elem -> elem.innerText <- $"{f.count}:\t{f.tag}")
            |> SuggestModal.withOnChooseSuggestion (fun (chosenResult,eventArgs) ->
                let cmd = plugin.settings.defaultModalCommand
                match plugin.app?commands?executeCommandById(cmd) with 
                | false -> 
                    Notice.show $"failed to run command: {cmd}, configure Default modal command in settings"
                | true -> 
                    match document.querySelector("input.prompt-input")  with 
                    | null -> Notice.show "plugin outdated"
                    | modalInput -> 
                        modalInput?value <- $"{chosenResult.tag} "
                        let ev = Browser.Event.Event.Create("input",null)
                        modalInput.dispatchEvent(ev) |> ignore
            )
            |> SuggestModal.openModal
            None
        )  

let rec foldedTagSearch (plugin:ExtendedPlugin<PluginSettings>) =
    Command.forMenu (nameof foldedTagSearch) "Folded search by Tag"
        (fun _ ->

            let startAcc = {|
                    Level = 1                 
                    Query = ""                 
                |} 
            
            let getVaultTags (state: {| Level: int; Query: string |}) = 
                plugin.app.vault.getMarkdownFiles()
                |> Seq.choose plugin.app.metadataCache.getFileCache
                |> Seq.choose (fun f ->
                    f.tags
                    |> Option.filter (fun tags ->
                        tags.Exists(fun f -> f.tag.StartsWith state.Query))
                )
                |> Seq.collect id
                |> Seq.groupBy (fun f ->
                    f.tag |> String.untilNthOccurrence state.Level '/' 
                )
                |> Seq.map (fun (tag,tags) -> tag, tags |> Seq.length)
            
            
            let rec createModal state =
                plugin.app
                |> SuggestModal.create
                |> SuggestModal.withGetSuggestions2 (fun queryInput ->
                    let query = obsidian.prepareQuery queryInput
                    let matches =
                        getVaultTags state
                        |> Seq.map (fun (tag,count) -> {|count=count;tag=tag|} )
                        |> Seq.choose (fun f -> obsidian.fuzzySearch(query,f.tag) |> Option.map (fun search -> f,search.score) )
                        |> (fun results -> 
                            match queryInput with 
                            | "" -> results |> Seq.sortByDescending (fun f -> (fst f).count)
                            | _ -> results |> Seq.sortByDescending snd
                        )
                        |> Seq.map fst
                    matches 
                    )
                |> SuggestModal.withRenderSuggestion (fun f elem ->
                    elem.innerText <- $"{f.count}:\t{f.tag}"
                )
                |> SuggestModal.withKeyboardShortcut
                    {
                      modifiers = []
                      key = "Tab"
                      action = (fun (evt,modal) ->
                          console.log "yello"
                        )
                    }
//                |> SuggestModal.map (fun sm ->
//                    sm.scope.register(
//                        null,
//                        Some "Tab",
//                        !!(fun (evt:KeyboardEvent) ->
//                            let currSelection =
//                                SuggestModal.getCurrentSelection sm
//                            let newState =
//                                {|
//                                  state with
//                                    Level = state.Level + 1
//                                    Query = currSelection.tag
//                                  |}
//                            sm.close()
//                            console.log newState
//                            createModal newState
//                            U2.Case1 false
//                        )
//                    )
//                    |> ignore
//                )
                |> SuggestModal.withOnChooseSuggestion (fun (chosenResult,eventArgs) ->
                    match eventArgs with
                    | U2.Case1 mouseEvent -> ()
                    | U2.Case2 keyboardEvent -> 
                        match keyboardEvent.key with
                        | "Enter" ->
                            //Browser.Dom.window.alert "hello"
                            console.log "default enter pressed"
                        | "Tab" -> Browser.Dom.window.alert "tab pressed"
                        | _ ->
                            console.log "something pressed"      
                        ()
                      
    //                let cmd = plugin.settings.defaultModalCommand
    //                match plugin.app?commands?executeCommandById(cmd) with 
    //                | false -> 
    //                    Notice.show $"failed to run command: {cmd}, configure Default modal command in settings"
    //                | true -> 
    //                    match document.querySelector("input.prompt-input")  with 
    //                    | null -> Notice.show "plugin outdated"
    //                    | modalInput -> 
    //                        modalInput?value <- $"{chosenResult.tag} "
    //                        let ev = Browser.Event.Event.Create("input",null)
    //                        modalInput.dispatchEvent(ev) |> ignore
                )
                //
                |> SuggestModal.mapResultContainer (fun f ->
                    let div = f :?> HTMLDivElement
                    div.onkeydown <- (fun keyboardEvent ->
                        match keyboardEvent.key with
                        | "Enter" ->
                            //Browser.Dom.window.alert "hello"
                            console.log "default enter pressed"
                        | "Tab" -> console.log "tab pressed"
                        | _ ->
                            console.log "something pressed"  
                    )
                    //div.style.backgroundColor <- "red"
                    ()
                    
                )
                |> SuggestModal.openModal
                
            createModal startAcc
                
            None
        )

let rec insertHeading4 (plugin:ExtendedPlugin<PluginSettings>) =
    Command.forEditor (nameof insertHeading4) "Insert heading 4"
        (fun edit ->
            edit.replaceSelection("#### ")
            doNone
        )            

let rec insertHeading5 (plugin:ExtendedPlugin<PluginSettings>) =
    Command.forEditor (nameof insertHeading5) "Insert heading 5"
        (fun edit ->
            edit.replaceSelection("##### ")
            doNone
        )     

let rec increaseHeading (plugin:ExtendedPlugin<PluginSettings>) =
    Command.forEditor (nameof increaseHeading) "Increase Heading level"
        (fun edit ->
            let lineIdx = edit.getCursor().line
            let currLine = lineIdx |> edit.getLine
            match currLine.StartsWith("#"), currLine.StartsWith("######") with 
            | false,_ | _, true -> doNone
            | _ -> 
                edit.setLine(lineIdx,$"#{currLine}")
                doNone
        )     

let rec decreaseHeading (plugin:ExtendedPlugin<PluginSettings>) =
    Command.forEditor (nameof decreaseHeading) "Decrease Heading level"
        (fun edit ->
            let lineIdx = edit.getCursor().line
            let currLine = lineIdx |> edit.getLine
            match currLine.StartsWith("##") with 
            | false -> doNone
            | _ -> 
                edit.setLine(lineIdx,currLine[1..])
                doNone
        )  

let rec insertDefaultCallout (plugin:ExtendedPlugin<PluginSettings>) =
    Command.forEditor (nameof insertDefaultCallout) "Insert Default Callout"
        (fun edit ->
            edit.replaceSelection($"> [!{plugin.settings.defaultCalloutType}] ")
            doNone
        )   
let rec insertCodeBlock (plugin:ExtendedPlugin<PluginSettings>) =
    Command.forEditor (nameof insertCodeBlock) "Insert Code Block"
        (fun edit ->
            edit.replaceSelection($"````{plugin.settings.defaultCodeBlockLanguage}\n\n````")
            let cursor = edit.getCursor()
            edit.setCursor(U2.Case2 (cursor.line - 1.))
            doNone
        )  
        
let rec insertTest (plugin:ExtendedPlugin<PluginSettings>) =
    
    Command.forEditor "testtesttest" "qqqqqq"
        (fun edit ->
            edit.replaceSelection("````\naaaaa\n````")
            let cursor = edit.getCursor()
            edit.setCursor(U2.Case2 (cursor.line - 1.))
            doNone
        )