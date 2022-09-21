open System.Diagnostics
open System.IO

type Process with
    static member runToStdout(pname,pargs,cwd) = 
        use p = new Process(StartInfo= ProcessStartInfo(fileName=pname,arguments=pargs,WorkingDirectory=cwd))
        p.Start() |> ignore
        p.WaitForExit()


let pwsh (command:string) =
    Process.runToStdout("pwsh",$@"-c ""{command}""", __SOURCE_DIRECTORY__ )

pwsh "cp -r dist quick-snippets-and-navigation"
pwsh "7z a quick-snippets-and-navigation.zip ./quick-snippets-and-navigation/ -r"
pwsh "rm -Recurse -Force quick-snippets-and-navigation"

