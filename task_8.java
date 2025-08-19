import java.util.Random;
import java.util.Arrays;
import java.util.stream.Stream;


class ComplexMultiThreadProcessing {
    private static final int SIZE = 1000000;
    private static final int THREADS = 4;
    private static final int chunkSize = SIZE / THREADS;
    private static final int[] data = new int[SIZE];
    private static volatile int sum = 0;
    
    private static void partialSum(int start, int end) {
        int localSum = Arrays.stream(data, start, end).sum();
        
        synchronized (ComplexMultiThreadProcessing.class) {
             sum += localSum;
        }
    }

    private static Thread runPartialSumCalc(int baseIndex)
    {
		 final int start = baseIndex * chunkSize;
		 final int end = (baseIndex + 1) * chunkSize;
		 Thread thread = new Thread(() -> { partialSum(start, end); });
		 thread.start();
		 return thread;
    }
	
    private static void joinThread(Thread thread)
    {
    	try {
              thread.join();
        } catch (InterruptedException e) 
        {
           e.printStackTrace();
        }
    }

    public static void main(String[] args) {
    	
        Random random = new Random();
        Arrays.setAll(data, i -> random.nextInt(100));

        Thread[] threads = new Thread[THREADS];
        
        Arrays.setAll(threads, i -> runPartialSumCalc(i));
        Arrays.stream(threads).forEach(thread -> joinThread(thread));

        System.out.println("Sum of all elements: " + sum);
    }
}
