func :: Double -> Double -> Double -> [Double]
func a b c | d > 0 = [x1,x2]
            | d < 0 = []
            | otherwise = [x1]
              where d=b*b-4*a*c
                    x1=((-b)+(sqrt d))/(2*a)
                    x2=((-b)-(sqrt d))/(2*a)
test :: Bool
test =
 let a = func 4 2 (-2)
     b = func 4 0 0
     c = func 10 2 1
 in a == [0.5,-1.0] && b == [0.0] && c == []