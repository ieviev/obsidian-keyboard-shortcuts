module Main

open Fable.Core
open Fs.Obsidian
open Microsoft.FSharp.Core
open Microsoft.FSharp.Reflection
open ObsidianBindings
open Fable.Core.JsInterop


[<Import("Plugin", from = "obsidian")>]
type BasePlugin(app, manifest) =
    do ()

type Plugin2(app, manifest) as instance =
    inherit BasePlugin(app, manifest)

    let plugin: ExtendedPlugin<PluginSettings> =
        instance :> obj :?> ExtendedPlugin<PluginSettings>

    let init () =
        plugin?loadSettings <- (fun _ ->
            promise {
                let! data = plugin.loadData ()

                match data with
                | None -> plugin.settings <- PluginSettings.Default
                | Some v ->
                    try
                        plugin.settings <- v :?> PluginSettings
                    with
                    | e -> plugin.settings <- PluginSettings.Default
            })

        plugin?saveSettings <- (fun settings ->
            promise {
                plugin.settings <- settings
                do! plugin.saveData (Some(!!settings))
            })

    do init ()

    let onload: unit -> unit =
        (fun _ ->
            // printJson "aciq:quick-snippets-and-navigation loaded"
            plugin.settings <- plugin.loadSettings ()

            SettingTab.create app plugin
            |> plugin.addSettingTab

            [|
                Commands.copyCodeBlock
                Commands.copyNextCodeBlock
                Commands.insertCodeBlock
                Commands.goToPrevHeading
                Commands.goToNextHeading
                Commands.insertHeading4
                Commands.insertDefaultCallout
                Commands.openSwitcherWithTag1
                // Commands.insertTest
            |]
            |> Seq.iter (fun cmd -> plugin |> cmd |> plugin.addCommand |> ignore)

            )

    do plugin?onload <- onload
