# Звіт про виконання лабораторної роботи №4
## Тема: Модульне ядро, Clean Architecture та Dependency Injection

**Навчальний заклад:** Херсонський національний технічний університет (ХНТУ)  
**Кафедра:** Програмних засобів і технологій  
**Викладач:** асистент Матвійчук Олексій Вадимович  
**Студент:** [ПІБ]  
**Група:** [Номер групи]  
**Тег у репозиторії:** lr4  
**Проєкт:** EventFlow — Online Ticket Booking & Event Platform

### 1. Шар і правило залежностей

Реалізовано чотири шари:

- `src/Domain`: сутність `Booking`, `BookingStatus`, `DomainException`, порт `IBookingRepository`.
- `src/Application`: `CreateBookingUseCase`, команди та результат сценарію, порт `ISeatAvailabilityPort`.
- `src/Infrastructure`: `InMemoryBookingRepository` та `InMemorySeatAvailabilityAdapter`.
- `src/WebAPI`: `BookingsController`, `Program.cs` та композиційний корінь DI.

Domain не містить посилань на Infrastructure, WebAPI, ORM або DI-контейнер. Application залежить лише від Domain. Реалізації портів розміщені в Infrastructure. WebAPI збирає залежності.

### 2. Сценарій використання

Основний сценарій — `CreateBookingUseCase.ExecuteAsync`, що відповідає операції `POST /v1/bookings` із контракту лабораторної №3.

Залежності передаються через конструктор:
- `IBookingRepository` — порт збереження бронювання;
- `ISeatAvailabilityPort` — порт перевірки доступності місць.

Доменні правила перевіряють ідентифікатори події та клієнта, кількість місць від 1 до 10, відсутність порожніх і повторюваних ідентифікаторів. Якщо правило порушено, запис не виконується.

### 3. Композиційний корінь

Реєстрації розташовані централізовано в `src/WebAPI/Program.cs`.

| Абстракція | Реалізація | Життєвий цикл | Причина |
|---|---|---|---|
| `IBookingRepository` | `InMemoryBookingRepository` | Singleton | Потокобезпечне in-memory сховище спільне для запитів |
| `ISeatAvailabilityPort` | `InMemorySeatAvailabilityAdapter` | Singleton | Адаптер не має стану запиту |
| `CreateBookingUseCase` | `CreateBookingUseCase` | Scoped | Окремий екземпляр на HTTP-запит |

Сценарій не створює адаптери через `new` і не використовує Service Locator.

### 4. Модульні тести

Тестовий проєкт: `tests/EventFlow.UnitTests`.

Використано xUnit та ручні заглушки `BookingRepositoryStub` і `AvailabilityStub`. Тести не запускають WebAPI, не підключають базу даних і не використовують мережу.

Перевірки:
1. `ExecuteAsync_ValidBooking_SavesThroughRepositoryPort` — успішний сценарій зберігає бронювання через порт.
2. `ExecuteAsync_InvalidBooking_DoesNotSave` — порушення доменного правила не викликає запис.
3. `ExecuteAsync_RepositoryFailure_PropagatesToCaller` — помилка порту передається виклику сценарію.

Лог запуску: `docs/lr4-test-results.txt`.

### 5. Висновки

Реалізовано модульне ядро EventFlow за принципами Clean Architecture. Напрямок залежностей спрямований до Domain, залежності сценарію передаються через конструктор, реалізації портів винесені в Infrastructure, а DI централізовано налаштований у WebAPI. Модульні тести ізольовано від інфраструктури.
