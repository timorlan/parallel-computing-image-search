# Parallel Computing & Image Search Project (C#)

## 📌 Overview
This repository contains **multithreaded C# applications** developed for an **Operating Systems project**:
1. **Multithreaded Matrix Multiplication** - Uses multiple threads to speed up matrix multiplication.
2. **Multithreaded Merge Sort** - Implements Merge Sort using parallel computing.
3. **Parallel Image Search** - Searches for a small image inside a larger image using **exact** and **Euclidean distance matching**.

---

## 🚀 Features

### 🏎️ Multithreaded Matrix Multiplication
- Performs **matrix multiplication** using **concurrent threads**.
- Supports **large matrices** (e.g., `1000x5000`).
- Optimized for **parallel execution**.

### 🔄 Multithreaded Merge Sort
- Implements **merge sort using threads**.
- Uses **fork and join strategy** for parallel sorting.
- Works efficiently with **large lists**.

### 🖼️ Parallel Image Search
- Finds a **small image inside a large image**.
- Supports **two search algorithms**:
  1. **Exact Match** - Uses **pixel-by-pixel** comparison.
  2. **Euclidean Distance Match** - Calculates **color similarity** based on pixel RGB values.
- Supports **multi-threading for fast searching**.

---

## 🛠️ How to Run

### 🔹 Matrix Multiplication & Merge Sort
1. Open `MatrixMultiplier.sln` or `MTMergeSort.sln` in **Visual Studio**.
2. Click **Start** to run.

### 🔹 Parallel Image Search (Command Line)
1. Open a **command prompt** in the `ImageSearch` folder.
2. Run the program:
   ```
   ImageSearch <bigimage.jpg> <smallimage.jpg> <nThreads> <algorithm>
   ```
   Example:
   ```
   ImageSearch big.jpg small.jpg 4 exact
   ```
3. The program will output **all occurrences** of the small image in the large image.

---
