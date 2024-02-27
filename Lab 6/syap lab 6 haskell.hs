import Data.List
import System.CPUTime
import Control.Monad
import Control.DeepSeq


type Polynomial = [(Int, Int)]

multiplyPolynomials :: Polynomial -> Polynomial -> Polynomial
multiplyPolynomials poly1 poly2 = 
    let multiplied = [ (c1*c2, e1+e2) | (c1, e1) <- poly1, (c2, e2) <- poly2 ]
--крафтим новый многочлен перемножением каждого члена первого многочлена со вторым
        grouped = groupBy (\(_, e1) (_, e2) -> e1 == e2) $ sortOn snd multiplied
--группируем список по кортежам, где степени равны
        summed = map (\g -> (sum $ map fst g, snd $ head g)) grouped
--преобразуем каждую группу пар в grouped в пару, где первый элемент - это сумма первых элементов всех пар в группе, а второй элемент
-- это второй элемент пары в группе
    in  filter ((/= 0) . fst) $ sortBy (\(_, e1) (_, e2) -> compare e2 e1) summed
--удаляем слагаемые у которых коэффициенты равны 0

printPolynomial :: Polynomial -> IO ()
printPolynomial [] = return ()
printPolynomial ((c, e):poly) = do
    putStr $ showTerm c e
    mapM_ (\(c, e) -> putStr (" + " ++ showTerm c e)) poly
--для красивого вывода каждого кортежа в полиноме выполняем условия showTerm
    putStrLn ""
  where
    showTerm c 0 = show c
    showTerm 1 1 = "x"
    showTerm c 1 = show c ++ "x"
    showTerm 1 e = "x^" ++ show e
    showTerm c e = show c ++ "x^" ++ show e

main :: IO ()
main = do
    let poly1 = [(1,1), (1,0)]
    let poly2 = [(1,1), (1,0)]
    start <- getCPUTime
    replicateM_ 10000 $ do
        let result = multiplyPolynomials poly1 poly2
        result `deepseq` return()
    end <- getCPUTime
    --let result = multiplyPolynomials poly1 poly2
    --printPolynomial result
    let diff = fromIntegral (end - start) / (10^9)
    putStrLn $ "Total runtime: " ++ show diff ++ " ms"
