# OneScript-MemoryCache
Библиотека, реализующая кэш на основе MemoryCache.Default

### OneScript
Установка осуществляется простым копированием файлов dll в какую-нибудь папку.

### HTTP-сервисы
Установка осуществляется копированием файлов dll в папку Bin веб-приложения.
Затем, необходимо подключить библиотеку, добавив нижеследующую строку в секцию <appSettings>, файла web.config:

```bsl
<add key="DefaultMemoryCache" value="attachAssembly" />
```

## Использование

### Подключение OneScript

```bsl
ПодключитьВнешнююКомпоненту("ПутьКПапкеГдеРасположеныDll\DefaultMemoryCache.dll");
```

### Подключение HTTP-сервисы OneScript
Поскольку dll библиотеки подключается автоматически при старте web-приложения, никаких действий не требуется.

### Пример использования

```bsl
// Создаем объект кэша
Кэш =  Новый КэшПамятиПоУмолчанию;

// Пытаемся получить объект по идентификатору
Результат = Кэш.Получить("c:\1\1\source.txt");

Если Результат = Неопределено Тогда

	// Объекта нет в кэше
	// Создаем политику кэширования, которая будет удалять объект из кэша при изменениии файла
	Политика = Новый ПолитикаКэшированияЭлемента;
	
	МассивФайлов = Новый Массив;
	МассивФайлов.Добавить("c:\1\1\source.txt");
	МассивМониторов = Новый Массив;
	МассивМониторов.Добавить(Новый МониторИзменеияФайловНаКомпьютере(МассивФайлов));
	Политика.МониторыИзменения = МассивМониторов;
	
	// Также, можем ограничить нахождение объекта в кэше определенной датой
	Политика.ДатаИстечения = ТекущаяДата() + 60;
	// Или удаляем объект, если он не использовался определенное количество секунд
	// Совместное использование ПериодНеИспользования и ДатаИстечения невозможно
	//Политика.ПериодНеИспользования = 10;
	
	// Создаем объект
	Результат = Новый ТекстовыйДокумент;
	Результат.Прочитать("c:\1\1\source.txt");
	
	// Помещаем его в кэш. Первый параметр - идентификатор объекта
	Кэш.Добавить("c:\1\1\source.txt", Результат, Политика);					

КонецЕсли;
```

## Установка
