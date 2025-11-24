# Snapshot Manager

## Introdution

Добрый день, Антон Валентинович!
Ниже приведена инструкция по запуску проекта и таблица, где расписаны элементы задания и где их искать.
Проект может показаться пустым, кривоватим и недоделанным, но это наработки моего дипломного проекта, который еще, конечно же, не закончен.
Сообщите пожалуйста результат проверки мне по email: "atupikov775@gmail.com"

## Start up steps:

1. Клонируйте репозиторий к себе на компьютер и запустите web-проект: "SnapManager" из солюшена "SnapManager.sln"
2. Проигнорируйте появившееся windows окно. Это толстая версия UI на WPF, которая не относится к заданию. Можете свернуть, но не закрывайте, это приведет к завершению программы.
3. Хост слушает запросы на порту 5005 (https://localhost:5005). Поэтому откройте браузер и перейдите по ссылке. Это и есть домашняя страница приложения.

## Task elements

<table border="1" cellspacing="0" cellpadding="8" style="border-collapse: collapse; width: 100%; font-family: Arial, sans-serif;">
  <thead style="background-color: #f0f0f0;">
    <tr>
      <th style="width: 1000px; text-align: left;">Текст задания</th>
      <th style="text-align: center;">Элемент(ы), содержащие реализацию</th>
      <th style="text-align: center;">Url адрес элемента</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>1. согласно контенту разработанного Вами Web-сайта необходимо использовать элементы семантической<br>разметки языка HTML5 (не менее 5 различных элементов);</td>
      <td>"...\SnapManager\Views\Shared\_Layout.cshtml"<br>"...\SnapManager\Views\Shared\_Navigation.cshtml"</td>
      <td>https://localhost:5005/</td>
    </tr>
    <tr>
      <td>2. примените не менее 5 различных новых правил CSS3 для стилевого оформления Вашего Web-сайта;</td>
      <td>"...\SnapManager\Views\Shared\_Layout.cshtml"<br>"...\SnapManager\Views\Configuration\_ConfigurationLayout.cshtml"<br>"...\SnapManager\wwwroot\styles\*.css"</td>
      <td>https://localhost:5005/<br>https://localhost:5005/Configuration/Feedback</td>
    </tr>    
    <tr>
      <td>3. на HTML-страницу обеспечивающий функциональность WebStorage;</td>
      <td>"...\SnapManager\wwwroot\scripts\layout.js"<br>"...\SnapManager\wwwroot\scripts\configuration-layout.js"</td>
      <td>https://localhost:5005/<br>https://localhost:5005/Configuration/DbSettings<br>https://localhost:5005/Configuration/Feedback</td>
    </tr>
    <tr>
      <td>4. продемонстрируйте на примере разработанного Вами Web-сайта возможность использования в стандарте<br>создания графики с помощью Canvas API или масштабируемой векторной графики SVG;</td>
      <td>"...\SnapManager\wwwroot\scripts\layout.js"<br>"...\SnapManager\Views\Shared\_Layout.cshtml"</td>
      <td>https://localhost:5005/</td>
    </tr>
    <tr>
      <td>5. с использованием тегов языка HTML5 (&lt;audio&gt; или &lt;video&gt;) подключите несколько<br>мультимедийных объектов на HTML-страницу Вашего Web-сайта;</td>
      <td>"...\SnapManager\Views\Configuration\Feedback.cshtml"</td>
      <td>https://localhost:5005/Configuration/Feedback</td>
    </tr>    
  </tbody>
</table>

⚠️ Примечание: "https://localhost:5005/Configuration/Feedback" - страница была добавлена исключительно для добора заданий, поэтому выглядит совсем из ряда вон. Но аудио очень рекомендую вам прослушать!

