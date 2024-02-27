counter :: [a] -> Int -> [[a]]
counter [x] _ = []
counter x n   = [xn]++(counter xn n) 
                where m  = length x
                      k  = n `mod` m
                      xn = if (k==0) then (init x) else  (drop k x) ++ (take (k-1) x)
test :: Bool
test = 
 let a = ["qwertyuiop","asd","asdasds","asfgadsdasd","asdasd","asldasdasd","affskmkm"]
     b = 5
     c = ["qwertyuiop"]
 in counter a b == [["asldasdasd","affskmkm","qwertyuiop","asd","asdasds","asfgadsdasd"],["asfgadsdasd","asldasdasd","affskmkm","qwertyuiop","asd"],["asfgadsdasd","asldasdasd","affskmkm","qwertyuiop"],["asldasdasd","affskmkm","qwertyuiop"],["qwertyuiop","asldasdasd"],["asldasdasd"]]


