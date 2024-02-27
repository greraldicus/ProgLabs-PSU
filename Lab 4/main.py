import ctypes
import numpy as np
import matplotlib.pyplot as plt


lib = ctypes.CDLL("./multipleby2.dll")
lib1 = ctypes.CDLL("./square.dll")
print("Two functions available: 1.multipleby2  2.square ")
print("Input the number of function: ")
choice = int(input())
if choice == 1:
    # Определяем типы аргументов и возвращаемого значения
    lib.multipleby2.argtypes = [np.ctypeslib.ndpointer(dtype=np.double),
                                       np.ctypeslib.ndpointer(dtype=np.double),
                                       ctypes.c_int]
    lib.multipleby2.restype = None

    # Вычисляем значение функции для некоторых значений аргумента
    x = np.linspace(-10, 10, 100)
    y = np.empty_like(x)
    lib.multipleby2(x, y, len(x))

    # Строим график функции
    plt.plot(x, y)
    plt.show()
elif choice == 2:
    # Определяем типы аргументов и возвращаемого значения
    lib1.square.argtypes = [np.ctypeslib.ndpointer(dtype=np.double),
                                       np.ctypeslib.ndpointer(dtype=np.double),
                                       ctypes.c_int]
    lib1.square.restype = None

    # Вычисляем значение функции для некоторых значений аргумента
    x = np.linspace(-10, 10, 100)
    y = np.empty_like(x)
    lib1.square(x, y, len(x))

    # Строим график функции
    plt.plot(x, y)
    plt.show()
else:
    print("Input is not correct")

