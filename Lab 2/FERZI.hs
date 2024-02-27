cross :: (Int,Int) -> (Int,Int) -> Bool
cross (x1,y1) (x2,y2) | x1 == x2 = True
                      | y1 == y2 = True
                      | abs(x1 - x2) == abs(y1 - y2) = True
                      | otherwise = False

crossMany :: (Int,Int) -> [(Int,Int)] -> Bool
crossMany pos list | null list = False
                   | cross pos (head list) = True
                   | otherwise = crossMany pos (tail list)

queens :: [(Int,Int)] -> Bool
queens [] = False
queens (x:xs) = (crossMany x xs) || queens xs

test :: Bool
test =
    let x1 = [(1,1),(2,3),(3,5),(4,7),(5,2),(6,4),(7,6),(8,8)]
        x2 = [(1,2),(7,7),(5,8)]
    in queens x1 == True &&
        queens x2 == False 