# Goose Defense

[Siberian Game Jam - May 2020](https://itch.io/jam/sibgamejam-may-2020)

<img src="Assets/icon8.png" alt="logo" width="360" height="360">

## Авторы(Authors)

- Statnikov Andey - programmer(core), manager
- Frolov Dmitry - programmer(core), manager
- Frolov Ivan - programmer(UI), sound designer
- Maidannik Nikita - programmer(animations, core)
- Tarasova Maria - CG art
- Tarasov Valeriy - CG animations

## Техническое задание

### Требование к системе

Unity: 2019.3.12f1

### Запуск игры:

Отображается замыленная карта мира с главным окном по центру, которое содержит краткий текст истории мира на сером полупрозрачном фоне: "Весь мир был поражён вирусом, из-за которого гуси стали неимоверно большими, голодными, и практически поработили человечество с помощью звона колокольчиков. Единственное, что осталось у людей - это большой колокол и фермы, на которых они выращивают корм чтобы кормить гусей. Это последняя надежда человечества. С минуты на минуты гуси начнут своё последнее наступление, защитите колокол!" и надпись, что
При нажатии на любую клавишу запускается игра.

### Начало игры:

Игроку даётся 15 секунд на начальное обустройство территории. Затем начинают спавниться гуси.

### Игровой процесс:

#### Цель игры:

Защитить колокол от гусей (при нашествии гуся-босса он может его украсть)

#### Гуси

Гуси движутся в направлении ближайшей цели (оборонительная башня, колокол) и кусают её, отнимая у неё прочность.
Имеют:

- Текущее здоровье (уровень голода)
- Максимальное здоровье (уровень голода)
- Размер (влияет на размер текстуры)
- Скорость
- Цель (позиция цели)
- Позиция
  Отображение:
- Здоровье (уровень сытости), полоска под гусём, появляется только при здоровье <100%
- Состояние: атакует, идёт, убегает, стоит

При здоровье (уровне голода) <= 0 гуси умирают (убегают с поля нападения).

#### Гусь-босс

Вызывается при сломе главной стены.

Идёт исключительно в сторону колокола, затем берёт его и уносит за пределы карты. Имеет повышенные размеры, не обращает внимание на другие цели.
Остальные параметры как у обычного гуся.

#### Стена

Имеется 2 линии стен. Каждая стена имеет ограниченный уровень прочности и номер.

#### Оборонительная башня

Имеет:

- Силу атаки
- Радиус атаки
- Дальность атаки
- Скорость снаряда
- Время перезарядка
- Позиция (номер точки)

Имеет заранее установленные позиции в 2 линии.
https://i.imgur.com/m3xeYJB.png

При нажатии на позицию для установки башни в информационное окно помещается информация об установленной башне: https://i.imgur.com/Wuj21u7.png
В случае, если нету установленной башни, отображается возможность установки:
https://i.imgur.com/FVEg4uR.png

При наведении на кнопку "улучшить" или "установить" отображаются характеристики устанавливаемой башни: https://i.imgur.com/eaqTjzK.png

#### Интерфейс

- Уровень угрозы (отображает текущий уровень сложности)
- Очки (количество накормленных гусей \* на их макс здоровье)
- Информационное окно (по стандарту выключено)
- Кнопка выхода
- Валюта
- (кнопка настроек, если понадобится)

#### Окончание игры

При победе/поражении по центру экрана выводится надпись "Вы победили!" или "Поражение" и "СЧЁТ: " на следующей строке, при этом фон затемняется и размывается
