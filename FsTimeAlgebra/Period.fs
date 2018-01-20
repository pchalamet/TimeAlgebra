module Period
open System

type Period<'t>(from : DateTime, size : int, value : 't) = class end
with
    member this.From = from
    
    member this.To = from.AddDays((float)size)
    
    member this.Value = value




let rec intersect<'t1, 't2, 't3> (periods : Period<'t1> list) (atom : Period<'t2>) (builder : 't1 -> 't2 -> 't3) =
    seq {
        match periods with
        | current :: tail when current.To <= atom.From -> 
            yield! intersect tail atom builder
        | current :: tail when current.From < atom.To ->
            let newValue = builder current.Value atom.Value
            let newFrom = max current.From atom.From
            let newTo = min current.To atom.To
            let newSize = (newTo - newFrom).Days
            yield Period(newFrom, newSize, newValue)
            yield! intersect tail atom builder
        | _ -> ()
    }



type Periodization<'t> private (periods : Period<'t> list) =
    let Empty = Periodization(list.Empty)

    member this.Periods = periods
    
    member this.TryGetValue (date : DateTime) =
        match periods |> Seq.tryFind (fun x -> x.From <= date & date < x.To) with
        | Some x -> Some x.Value
        | _ -> None
    
    member this.Intersect<'t2, 't3> (period : Period<'t2>) (builder : 't -> 't2 -> 't3) =
        intersect periods period builder

    member this.Union<'t2> (period : Period<'t2>)