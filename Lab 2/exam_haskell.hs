-- Задача 8
hash m [] = 0
hash m(h:t) = m h + hash f t
m x = x % 3

-- 6 8 10 12 14
-- 0 + 2 + 1 + 0 + 2 + 0 = 5 
-- Ответ: 5

-- Задача 9
mean :: [Double] -> Double
mean xs = go xs 0 0
  where
    go [] s n = s / n
    go (x:xs) s n = go xs (s+x) (n+1)
