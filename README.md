![maze_menu](https://github.com/DaniilSob2004/Maze/assets/106149184/c556cdd2-d629-49fa-b1ad-16763c82ac80)
![maze](https://github.com/DaniilSob2004/Maze/assets/106149184/bea07926-05b3-4425-bee9-f1299d861d10)

https://github.com/DaniilSob2004/Maze/assets/106149184/0879f1fd-9242-4df4-b78e-be6f456dc2e6


- Элементы:
  - Коридор ![hall](https://github.com/DaniilSob2004/Maze/assets/106149184/862bda1c-dcf2-45bd-b845-0b06b6eaa5f7)
  - Стена ![wall](https://github.com/DaniilSob2004/Maze/assets/106149184/f43a9982-84d8-4156-8dfb-2b7eccabe87a)
  - Медаль ![medal](https://github.com/DaniilSob2004/Maze/assets/106149184/8da03708-23c9-446e-a103-e52e7c467bd1)
  - Враг ![enemy](https://github.com/DaniilSob2004/Maze/assets/106149184/96b8ffb6-ced6-469e-9957-74ed74278b06)
  - Игрок ![player](https://github.com/DaniilSob2004/Maze/assets/106149184/097f08b6-b004-44bd-8c2e-d54b55a09a26)
  - Лекарство ![pill](https://github.com/DaniilSob2004/Maze/assets/106149184/cf723ba8-ad8f-4d29-ad40-99ffedf07b16)
  - Энергетик ![energy](https://github.com/DaniilSob2004/Maze/assets/106149184/510eb17c-8bfe-4482-ba65-e898f5be7b4e)
  - Бомба ![bomb](https://github.com/DaniilSob2004/Maze/assets/106149184/85f356f8-1c2f-47d5-a803-59ed2c7b9d9a)
  - Взрыв ![detonation](https://github.com/DaniilSob2004/Maze/assets/106149184/8f27f22a-abf9-4b9f-afaf-9a807f8f8413)


- Статистика(Кол-во):
  - Медалей
  - Здоровья(максимально 100%)
  - Энергии(максимально 500)
  - Врагов


- Кнопки управления:
  - Начать игру (меню) - запуск игры
  - Выйти из игры (меню)
  - Up(кнопка) - двигает игрока наверх
  - Down(кнопка) - двигает игрока вниз
  - Left(кнопка) - двигает игрока налево
  - Right(кнопка) - двигает игрока вправо
  - Esc(кнопка) - другая генерация лабиринта
  - Enter(кнопка) - игрок устанавливает бомбу


- Правила:
  - Каждое перемещение игрока, -1 энергия
  - При каждом 20 перемещении игрока, добавляется враг
  - При столкновении с врагом, -20% жизни
  - При установке бомбы, -50 энергии
  - Выпить лекарство можно, когда здоровье < 100%
  - Если выпил лекарство, +10% здоровья
  - Энергетик можно выпить, только после 10 перемещений с момента принятия лекарства
  - Если выпил энергетик, +25 энергии
  - При получении медали, прибавляется +1 к статистики


- Победа:
  - Если собраны все медали
  - Если убиты все враги


- Поражение:
  - Если жизнь 0%
  - Если взорвался на бомбе
