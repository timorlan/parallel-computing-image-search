using System;
using System.Collections.Generic;
using System.Threading.Tasks;

class MTMergeSort
{
    // Performs the multi-threaded merge-sort
    // Gets list of strings as input (strList) and the minimum number that each thread sorts (nMin)
    public List<string> MergeSort(string[] strList, int nMin = 2)
    {
        if (strList == null || strList.Length == 0)
            return new List<string>();

        string[] result = MergeSortInternal(strList, nMin);
        return new List<string>(result);
    }

    private string[] MergeSortInternal(string[] strList, int nMin)
    {
        if (strList.Length <= nMin)
        {
            Array.Sort(strList, string.Compare);
            return strList;
        }

        int mid = strList.Length / 2;
        string[] left = new string[mid];
        string[] right = new string[strList.Length - mid];

        Array.Copy(strList, 0, left, 0, mid);
        Array.Copy(strList, mid, right, 0, strList.Length - mid);

        Task<string[]> leftTask = Task.Factory.StartNew(() => MergeSortInternal(left, nMin));
        Task<string[]> rightTask = Task.Factory.StartNew(() => MergeSortInternal(right, nMin));

        Task.WaitAll(leftTask, rightTask);

        return Merge(leftTask.Result, rightTask.Result);
    }

    private string[] Merge(string[] left, string[] right)
    {
        string[] result = new string[left.Length + right.Length];
        int i = 0, j = 0, k = 0;

        while (i < left.Length && j < right.Length)
        {
            if (string.Compare(left[i], right[j]) <= 0)
            {
                result[k++] = left[i++];
            }
            else
            {
                result[k++] = right[j++];
            }
        }

        while (i < left.Length)
        {
            result[k++] = left[i++];
        }

        while (j < right.Length)
        {
            result[k++] = right[j++];
        }

        return result;
    }
}
