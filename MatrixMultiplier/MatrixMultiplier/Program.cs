class Program
{
    static void Main()
    {
        int rowsA = 1000;
        int colsA = 500;
        int colsB = 5000;
        int numThreads = 20;

        int[,] matrixA = new int[rowsA, colsA];
        int[,] matrixB = new int[colsA, colsB];
        int[,] resultMatrix = new int[rowsA, colsB];

        // Initialize matrices with some values
        Random rand = new Random();
        for (int i = 0; i < rowsA; i++)
        {
            for (int j = 0; j < colsA; j++)
            {
                matrixA[i, j] = rand.Next(10);
            }
        }

        for (int i = 0; i < colsA; i++)
        {
            for (int j = 0; j < colsB; j++)
            {
                matrixB[i, j] = rand.Next(10);
            }
        }

        // Perform concurrent matrix multiplication
        MatrixMultiplier.MultiplyMatricesConcurrently(matrixA, matrixB, resultMatrix, rowsA, colsA, colsB, numThreads);

        // Print the result matrix if needed (or just a part of it)
        Console.WriteLine("Matrix multiplication completed.");
    }
}
