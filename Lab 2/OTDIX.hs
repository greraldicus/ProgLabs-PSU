import Data.List
import Data.Function

mostCommon = head.maximumBy(compare `on` length).group.sort

maxVacations list = mostCommon (map snd list)

test :: Bool
test =
    let x1 = [(1,2), (2,1), (30, 1), (20, 3)]
        x2 = [(26, 1), (1, 2), (2, 2), (3, 3)]
        x3 = [(1, 12), (2, 12)]
        x4 = [(1,1),(2,1),(3,1),(1,2),(2,2),(3,2),(4,2),(5,2),(1,3)]
    in maxVacations x1 == 1 &&
        maxVacations x2 == 2 &&
        maxVacations x3 == 12 &&
        maxVacations x4 == 2

