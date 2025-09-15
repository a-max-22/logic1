Реализация quicksort на C#:
```csharp
    public class QuickSort
    {
        private static readonly Random _random = new Random();

        public static void quickSort(int[] arr)
        {
            if (arr == null || arr.Length <= 1)
                return;
            
            SortRecursive(arr, 0, arr.Length - 1);
        }

        private static void SortRecursive(int[] arr, int low, int high)
        {
            if (low >= high)
                return;
            
            int pivotIndex = Partition(arr, low, high);
            SortRecursive(arr, low, pivotIndex - 1);
            SortRecursive(arr, pivotIndex + 1, high);
        }

        private static int Partition(int[] arr, int low, int high)
        {
            int pivotIndex = _random.Next(low, high + 1);
            Swap(arr, pivotIndex, high);
            
            int pivot = arr[high];
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                if (arr[j] <= pivot)
                {
                    i++;
                    Swap(arr, i, j);
                }
            }
            
            Swap(arr, i + 1, high);
            return i + 1;
        }

        private static void Swap(int[] arr, int i, int j)
        {
            int temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }
    };
```

### Доказательства: 

Доказательство для Swap() опустим, так как оно очевидное. Единственное,  нужно учитывать корректность предусловия: `0 <= i,j < arr[arr.len]`.

Доказательство для SortRecursive:
1) Предусловие: low < high
2) Инвариант: для любого 0 < i <= low, low + 1 <= j < high выполняется arr[i] <= arr[j]
3) Постусловие: arr[i] < arr[j], для любого 0 <= i < j < arr.len 

Доказательство для Partition:
Предусловие: `arr.len >= 0, low < high`  
   
Постусловие:   в итоговом массиве существует такой индекс `pivot_index`, что:
	- для  любого i < `pivot_index`,  `arr[i] <= pivot`
	- для любого  j > `pivot_index`,  `arr[j] >= pivot`



#### Доказательство корректности цикла:
Предусловие:
- `pivot = arr[high]`
- `i = low - 1`
- `j = low`

Инвариант цикла:
- `arr[low..i] ≤ pivot`
- `arr[i+1..j-1] > pivot`

Докажем, что при выполнении инварианта цикла  :
1. Инициализация: перед итерацией выполняется условие:
    - `i = low-1`, `j = low`
    - `arr[low..low-1]` не содержит элементов,
    - `arr[low..low-1]` не содержит элементов,
   2. Процесс:  Если `arr[j+1] ≤ pivot`, то выполняется операция Swap(`arr[i+1], arr[j])` т.е. эти элементы меняются местами.  Таким образом каждый элемент `arr[low..i+1]` <= `pivot`.  Кроме того, из за операции swap `arr[i+2..j+1]  > pivot`. В силе того, что на этой итерации `i = i+1`, `j = j+1`, инвариант в этом случае остается  истинным. Если  же `arr[j + 1]` > `pivot`, то `arr[low..i] ≤ pivot` - не изменяется, а `arr[i+1..j]` > `pivot`. Таким образом, инвариант остается истинным и в этом случае.
3. Завершение: если  j = high, то:
    - `arr[low..i] ≤ pivot`
    - `arr[i+1..high-1] > pivot`
    - После перемены мест  `arr[i+1]` и  `arr[high]`,получаем:
        - `arr[low..i+1]` ≤ `pivot`
        - `arr[i+2..high]` > `pivot`
        - `pivot is now at position i+1`
   
