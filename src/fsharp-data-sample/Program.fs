module FSharpDataSample

open FSharp.Data
// open FSharp.Charting

module Say =
    let hello name =
        sprintf "Hello %s" name

module WorldBank =
    let data = WorldBankData.GetDataContext()

    let DoQuery () =
        let results =
            data
                .Countries.``Papua New Guinea``
                .Indicators.``Access to electricity, rural (% of rural population)``
            |> Seq.sortByDescending fst

        let numValues = Seq.length results
        if  numValues = 0 then
            results
        else
            let maxValues = min numValues 10
            results
            |> Seq.take maxValues

module Xml =
    [<Literal>]
    let SampleFileName = "fsharp-data-sample.fsproj"
    type FsProj = XmlProvider<SampleFileName>
    let proj = FsProj.Load("../../../fsharp-data-sample.fsproj") //xmlFileName
    let GetSdk () = proj.Sdk
    let GetTarget () = proj.PropertyGroups.[0].TargetFramework.Value

[<EntryPoint>]
let main _ =
    let worldBankResults = WorldBank.DoQuery ()
    worldBankResults
    |> Seq.iter (fun (year, value) -> printfn "Year: %i; Value: %f" year value)
    // worldBankResults
    // |> Chart.Line // This does not work in a dotnet core 2.0 app.
    // |> ignore
    printfn "Project Sdk: %s" <| Xml.GetSdk ()
    printfn "Project Target Framework: %s" <| Xml.GetTarget ()

    0 // return an integer exit code
