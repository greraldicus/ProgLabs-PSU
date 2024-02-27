
isSelfDual :: Eq a => [a] -> Int -> Int -> Bool
isSelfDual list a b
    |a > b = True
    |list !! a == list !! b = False
    |list !! a /= list !! b = isSelfDual list (a + 1) (b - 1)

isSelfDualCheck list = isSelfDual list a b
    where a = 0; b = length list - 1

test :: Bool
test =
    let x1 = [1,1,0,0]
        x2 = [1,0,0,1]
        x3 = [1,1,1,1,0,0,0,0]
        x4 = [0,0]
    in isSelfDualCheck(x1) && 
    isSelfDualCheck(x2) == False &&
    isSelfDualCheck(x3) &&
    isSelfDualCheck(x4) == False


