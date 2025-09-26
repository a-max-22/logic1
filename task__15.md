В качестве примера собственного паттерна/абстракции можно привести вспомогательный интерфейс `BackedUp`: 
```python
class BackedUp:
  def __init__(self):
      pass
  def backup(self, data:object)
      # save data
      ...
  def restore(self, data:object)
      # save data
  
```

Класс, реализующие нужные вычисления может наследоваться от  данного класса (наподобие схемы с миксинами). В ходе длительных вычислений можно сохранять промежуточные результаты
в некотором внешнем хранилище например для того, чтобы не терять прогресс вычислений в случае непредвиденного завершения.  

```python
class LongCalcs(BackedUp):
  ...
  def do_calc(self):
    ...
    while (not calc_finished):
      self.backup(data)
```
