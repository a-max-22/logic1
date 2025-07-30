### Первый пример
В первом примере состояние гонки возникает из-за того, что несколько потоков могут одновременно читать и записывать общую переменную `counter`. В результате, например, происходит ситуация, когда один поток считал и увеличил `counter` на 1, записав в переменную новое значение - `counter + 1`. В это же время другой поток, считав counter так же увеличивает его на 1 и записывает. По итогу работы двух потоков, в переменной counter будет значение не `counter + 2`, а `counter + 1`. Чтобы исправить ситуацию необходимо либо вводить механизм синхронизации, либо производить увеличение на 1 атомарно, например через  `AtomicInteger.getAndIncrement`:
```java
public class RaceConditionExample {

    private static AtomicInteger counter;

    public static void main(String[] args) {
        int numberOfThreads = 10;
        Thread[] threads = new Thread[numberOfThreads];

	    counter.set(0)
        for (int i = 0; i < numberOfThreads; i++) {
            threads[i] = new Thread(() -> {
                for (int j = 0; j < 100000; j++) {
                    counter.getAndIncrement();
                }
            });
            threads[i].start();
        }

        for (int i = 0; i < numberOfThreads; i++) {
            try {
                threads[i].join();
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }

        System.out.println("Final counter value: " + counter);
    }
}
```


### Второй пример
Во втором примере взаимная блокировка возникает из-за того, что первый поток стартует и  блокирует `lock1`. При этом второй поток стартует и блокирует `lock2`. Далее первый поток также  пытается блокировать `lock2` и ожидает, пока тот освободится. В это время второй поток пытается блокировать `lock1` и так же находится в режиме ожидания. В итоге ожидание будет вечным, так как первый поток, находясь в режиме ожидания, никогда не разблокирует `lock1`, а второй поток - `lock2`. 

Чтобы избежать этого, можно поменять местами порядок блокирования объектов синхронизации во второй функции - сначала lock1, а потом lock2. В таком случае общая логика функции останется неизменной, а взаимная блокировка будет невозможной по той причине, что не будет существовать пути выполнения кода, когда функция сможет  заблокировать `lock2`, не заблокировав при этом `lock1`:

```java
public class DeadlockExample {

    private static final Object lock1 = new Object();
    private static final Object lock2 = new Object();

    public static void main(String[] args) {
        Thread thread1 = new Thread(() -> {
            synchronized (lock1) {
                System.out.println("Thread 1 acquired lock1");

                try { Thread.sleep(50); } 
                catch (InterruptedException e) { e.printStackTrace(); }

                synchronized (lock2) {
                    System.out.println("Thread 1 acquired lock2");
                }
            }
        });

        Thread thread2 = new Thread(() -> {
	        // блокируем lock1, а не lock2
            synchronized (lock1) {
                System.out.println("Thread 2 acquired lock1");

                try { Thread.sleep(50); } 
                catch (InterruptedException e) { e.printStackTrace(); }

                synchronized (lock2) {
                    System.out.println("Thread 2 acquired lock2");
                }
            }
        });

        thread1.start();
        thread2.start();

        try {
            thread1.join();
            thread2.join();
        } catch (InterruptedException e) {
            e.printStackTrace();
        }

        System.out.println("Finished");
    }
}
```
