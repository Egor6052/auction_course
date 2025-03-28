### Variant for laboratory work: 13
---
- [Variant for laboratory work: 13](#variant-for-laboratory-work-13)
- [Build the project:](#build-the-project)
- [Installing SDK and runtime machine](#installing-sdk-and-runtime-machine)
- [Errors](#errors)


---
### Build the project:
```sh
dotnet build
```
For run, going to the build directory ```cd ./NameProject```, and:
```sh
dotnet run
```

### Installing SDK and runtime machine
---
For installing SDK in Fedora Linux: 
```sh
sudo dnf install dotnet-sdk-9.0
```
Install the runtime
```sh
sudo dnf install aspnetcore-runtime-9.0
```
and
```sh
sudo dnf install dotnet-runtime-9.0
```

### Errors
--- 
1xx – Інформаційні відповіді

    100 Continue – сервер отримав запит і очікує на відправку його тіла.

    101 Switching Protocols – сервер змінює протокол з HTTP на WebSockets або інший.

    103 Early Hints – сервер передає заголовки до основної відповіді.

2xx – Успішні відповіді

    200 OK – запит виконано успішно.

    201 Created – успішно створено новий ресурс.

    202 Accepted – запит прийнято, але ще не виконано.

    204 No Content – запит успішний, але немає вмісту у відповіді.

3xx – Перенаправлення

    301 Moved Permanently – ресурс переміщено назавжди.

    302 Found – ресурс тимчасово переміщено.

    304 Not Modified – ресурс не змінювався, можна використовувати кешовану версію.

4xx – Помилки клієнта

    400 Bad Request – некоректний запит (невірний синтаксис, відсутні параметри).

    401 Unauthorized – потрібна автентифікація користувача.

    403 Forbidden – у користувача немає прав доступу до ресурсу.

    404 Not Found – ресурс не знайдено.

    405 Method Not Allowed – метод (GET, POST тощо) не підтримується для ресурсу.

    408 Request Timeout – сервер чекав занадто довго і закрив з'єднання.

    429 Too Many Requests – користувач надіслав занадто багато запитів (захист від DDOS).

5xx – Помилки сервера

    500 Internal Server Error – загальна помилка сервера.

    501 Not Implemented – сервер не підтримує запитаний метод.

    502 Bad Gateway – сервер отримав неправильну відповідь від іншого сервера.

    503 Service Unavailable – сервер тимчасово недоступний (перевантаження, техобслуговування).

    504 Gateway Timeout – сервер не отримав відповідь від іншого сервера вчасно.