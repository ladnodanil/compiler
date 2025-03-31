# Постановка задачи
1. Разработать автоматную грамматику.
1. Спроектировать граф конечного автомата (перейти от автоматной грамматики к конечному автомату).
1. Выполнить программную реализацию алгоритма работы конечного автомата.
1. Встроить разработанную программу в интерфейс текстового редактора, созданного на первой лабораторной работе.

# Персональный вариант
Тема: Объявление ассоциативного массива языка C#

Пример верной строки
```c#
Dictionary<int, string> My_dict1 = new Dictionary<int, string>();

```
# Примеры допустимых строк
```c#
// пример варианта
Dictionary<int, string> My_dict1 = new Dictionary<int, string>();
// без пробелов
Dictionary<int,int>LOLOL=new Dictionary<int,int>();
// c отступами
Dictionary
<
int
,
int
>
LOLOL
=
new 
Dictionary
<
int
,
int
>
(

)
;
```
# Грамматика языка
Определим грамматику объявления ассоциативного массива языка C# G[‹START›] в нотации Хомского с продукциями P:
1)	`<START>`→’Dictionary’`<GENERIC_TYPE>`
2)	`<GENERIC_TYPE>`→’<’`<TKEY>`
3)	`<TKEY>`→`<type><COMMA>`
4)	`<COMMA>`→’,’`<TVALUE>`
5)	`<TVALUE>`→ `<type><CLOSE_GENERIC>`
6)	`<CLOSE_GENERIC>`→’>’`<ID>`
7)	`<ID>`→`<letter><IDREM>`
8)	`<IDREM>`→`<letter>`|`<digit><IDREM>`
9)	`<IDREM>`→’=’`<NEW>`
10)	`<NEW>`→’new’`<SPASE>`
11)	`<SPASE>`→’ ‘`<DICT_CREATTION>`
12)	`<DICT_CREATTION>`→’Dictionary’`<GENERIC_TYPE2>`
13)	`<GENERIC_TYPE2>`→’<’`<TKEY2>`
14)	`<TKEY2>`→`<type><COMMA2>`
15)	`<COMMA2>`→’,’`<TVALUE2>`
16)	`<TVALUE2>`→ `<type><CLOSE_GENERIC2>`
17)	`<CLOSE_GENERIC2>`→’>’`<OPEN_PAREN>`
18)	`<OPEN_PAREN>`→’(‘`<CLOSE_PAREN>`
19)	`<CLOSE_PAREN>`→’)’`<END>`
20)	`<END>`→’;’
- `<type>`→’int’|’string’
- `<letter>`→ ‘a’ | ‘b’ | ‘c’| ... | ‘z’ | ‘A’ | ‘B’ | ‘C’| ... | ‘Z’
- `<digit>`→’0’|’1’|…|’9’

Следуя введенному формальному определению грамматики, представим G[‹START›] ее составляющими:
- Z = ‹START›;
- VT = {a, b, c, ..., z, A, B, C, ..., Z,  , < , > , ( , ) , ; , , , = ,  , 0, 1, 2, ..., 9};
- VN = {`<START>`, `<GENERIC_TYPE>`, `<TKEY>`, `<COMMA>`, `<TVALUE>`, `<CLOSE_GENERIC>`, `<ID>`, `<IDREM>`, `<NEW>`, `<SPASE>`, `<DICT_CREATTION>`, `<GENERIC_TYPE2>`, `<TKEY2>`, `<COMMA2>`, `<TVALUE2>`, `<CLOSE_GENERIC2>`, `<OPEN_PAREN>`, `<CLOSE_PAREN>`, `<END>`, `<type>`, `<letter>`, `<digit>`}

# Классификация грамматики
Согласно классификации Хомского, грамматика G[‹START›] является автоматной.
Все правила (1)-(20) относятся к классу праворекурсивных продукций (A → aB | a | ε):
1)	`<START>`→’Dictionary’`<GENERIC_TYPE>`
2)	`<GENERIC_TYPE>`→’<’`<TKEY>`
3)	`<TKEY>`→`<type><COMMA>`
4)	`<COMMA>`→’,’`<TVALUE>`
5)	`<TVALUE>`→ `<type><CLOSE_GENERIC>`
6)	`<CLOSE_GENERIC>`→’>’`<ID>`
7)	`<ID>`→`<letter><IDREM>`
8)	`<IDREM>`→`<letter>`|`<digit><IDREM>`
9)	`<IDREM>`→’=’`<NEW>`
10)	`<NEW>`→’new’`<SPASE>`
11)	`<SPASE>`→’ ‘`<DICT_CREATTION>`
12)	`<DICT_CREATTION>`→’Dictionary’`<GENERIC_TYPE2>`
13)	`<GENERIC_TYPE2>`→’<’`<TKEY2>`
14)	`<TKEY2>`→`<type><COMMA2>`
15)	`<COMMA2>`→’,’`<TVALUE2>`
16)	`<TVALUE2>`→ `<type><CLOSE_GENERIC2>`
17)	`<CLOSE_GENERIC2>`→’>’`<OPEN_PAREN>`
18)	`<OPEN_PAREN>`→’(‘`<CLOSE_PAREN>`
19)	`<CLOSE_PAREN>`→’)’`<END>`
20)	`<END>`→’;’

# Граф конечного автомата
![graph](https://raw.githubusercontent.com/ladnodanil/compiler/laba3/compiler/icon/%D0%B3%D1%80%D0%B0%D1%84%20%D0%BA%D0%BE%D0%BD%D0%B5%D1%87%D0%BD%D0%BE%D0%B3%D0%BE%20%D0%B0%D0%B2%D1%82%D0%BE%D0%BC%D0%B0%D1%82%D0%B0.png)



# Тестовые примеры
