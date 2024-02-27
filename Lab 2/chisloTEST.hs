factorial n = if n == 0 then 1 else n * factorial (n - 1)
chislosochitanie n m = if m > n then 0 else (factorial n) / ((factorial (n - m)) * (factorial m))
test :: Bool
test = 
 let a = chislosochitanie 5 4
     b = chislosochitanie 1 2
     c = chislosochitanie 1 0
 in a == 5 && b == 0 && c == 1