module Main

open Fable.Core
open Fs.Obsidian
open Microsoft.FSharp.Core
open Fable.Core.JsInterop
open Fable.Core.JS


[<Import("Plugin", from = "obsidian")>]
type BasePlugin(app, manifest) =
    do ()

type Plugin2(app, manifest) as instance =
    inherit BasePlugin(app, manifest)

    let plugin: ExtendedPlugin<PluginSettings> =
        instance :> obj :?> ExtendedPlugin<PluginSettings>

    let init () =
        plugin?loadSettings <-
            (fun _ ->
                promise {
                    let! data = plugin.loadData ()

                    match data with
                    | None -> plugin.settings <- PluginSettings.Default
                    | Some v ->
                        try
                            plugin.settings <- v :?> PluginSettings
                        with e ->
                            plugin.settings <- PluginSettings.Default
                }
            )

        plugin?saveSettings <-
            (fun settings ->
                promise {
                    plugin.settings <- settings
                    do! plugin.saveData (Some(!!settings))
                }
            )

    do init ()

    let commandById (id: string) =
        plugin.app?commands?executeCommandById id

    let onload: unit -> unit =
        fun _ ->
            printfn "loaded:"
            plugin.settings <- plugin.loadSettings ()
            let settingsTab = SettingTab.create app plugin


            settingsTab |> plugin.addSettingTab

            let tagSearch = plugin.addCommand (Commands.tagSearch plugin)
            let copyCodeBlock = plugin.addCommand (Commands.copyCodeBlock plugin)
            let copyNextCodeBlock = plugin.addCommand (Commands.copyNextCodeBlock plugin)

            let goToPrevEmptyLine = plugin.addCommand (Commands.goToPrevEmptyLine plugin)
            let goToNextEmptyLine = plugin.addCommand (Commands.goToNextEmptyLine plugin)

            let goToPrevHeading = plugin.addCommand (Commands.goToPrevHeading plugin)
            let goToNextHeading = plugin.addCommand (Commands.goToNextHeading plugin)
            let increaseHeading = plugin.addCommand (Commands.increaseHeading plugin)
            let decreaseHeading = plugin.addCommand (Commands.decreaseHeading plugin)
            let insertDefaultCallout = plugin.addCommand (Commands.insertDefaultCallout plugin)
            let insertCodeBlock = plugin.addCommand (Commands.insertCodeBlock plugin)
            let foldedTagSearch = plugin.addCommand (Commands.foldedTagSearch plugin)

            plugin.addRibbonIcon (
                "layers-2",
                "Quick snippets and navigation: Search tags",
                (fun v ->
                    console.log settingsTab
                    plugin.app?commands?executeCommandById (tagSearch.id)
                    None
                )
            )
            |> ignore
           

    do plugin?onload <- onload
