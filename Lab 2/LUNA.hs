import Data.Char(digitToInt)
checkMoreThan x 
    |x > 9 = x - 9
    |otherwise = x


luhnAlgo :: String -> Int
luhnAlgo [] = 0
luhnAlgo (x:y:xs) = digitToInt x + checkMoreThan (2 * (digitToInt y)) + luhnAlgo xs

checkSum :: Int -> Bool
checkSum x = if (x `mod` 10 == 0) then True else False

isLuhnCorrect :: [Char] -> Bool
isLuhnCorrect l1 = if (length l1 `mod` 2 /= 0) then False else checkSum(luhnAlgo (reverse l1))

test :: Bool
test =
    let x1 = "2202203237721255"
        x2 = "4274320062586626"
        x3 = "2313212412412942"
        x4 = "123321"
        x5 = "4"
    in isLuhnCorrect x1 == True && isLuhnCorrect x2== True && isLuhnCorrect x3 == False && isLuhnCorrect x4 == False && isLuhnCorrect x5 == False
