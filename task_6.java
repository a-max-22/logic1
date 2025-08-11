import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;

/*
  В данном случае, для парсинга даты используется класс LocalDateTime, который в отличие от SimpleDateFormat
   1) Явно инициирует исключение, в случае, если указана некорректная дата, как например "2024-13-12".  
      В изначальном варианте дата распарсилась бы как  12 января 2025 года;
   2) Явно прописывается в коде, какая временная зона используется. 
      В нашем случае, используется локальное время при парсинге даты;
   3) Методы одного экземпляра SimpleDateFormat нельзя вызывать в рамках разных потоков. 
      В результате того, что два потока могут одновременно менять состояние класса 
      есть риск выдачи некорректного результата.   
*/

class DateExample {
    private static final DateTimeFormatter FORMATTER = 
        DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss");

    public static void main(String[] args) {
        String dateString = "2024-12-13 14:30:00";
        
        LocalDateTime date = LocalDateTime.parse(dateString, FORMATTER);
        System.out.println("Date: " + date);
    }
}
