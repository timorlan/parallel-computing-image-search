using System;
using System.Threading;

class MatrixMultiplier
{
    public static void MultiplyMatricesConcurrently(int[,] matrixA, int[,] matrixB, int[,] resultMatrix, int rowsA, int colsA, int colsB, int numThreads)
    {
        int rowsPerThread = rowsA / numThreads;
        Thread[] threads = new Thread[numThreads];

        for (int i = 0; i < numThreads; i++)
        {
            int startRow = i * rowsPerThread;
            int endRow = (i == numThreads - 1) ? rowsA : startRow + rowsPerThread;

            threads[i] = new Thread(() => MultiplyPart(matrixA, matrixB, resultMatrix, startRow, endRow, colsA, colsB));
            threads[i].Start();
            Console.WriteLine($"Thread {i + 1} started for rows {startRow} to {endRow}");
        }

        for (int i = 0; i < numThreads; i++)
        {
            threads[i].Join();
            Console.WriteLine($"Thread {i + 1} finished");
        }
    }

    private static void MultiplyPart(int[,] matrixA, int[,] matrixB, int[,] resultMatrix, int startRow, int endRow, int colsA, int colsB)
    {
        for (int i = startRow; i < endRow; i++)
        {
            for (int j = 0; j < colsB; j++)
            {
                int sum = 0;
                for (int k = 0; k < colsA; k++)
                {
                    sum += matrixA[i, k] * matrixB[k, j];
                }
                resultMatrix[i, j] = sum;
            }
        }
    }
}
