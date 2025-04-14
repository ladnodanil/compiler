# Постановка задачи
Реализовать алгоритм нейтрализации синтаксических ошибок и дополнить им программную реализацию парсера.

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
![граф конечного автомата](https://github.com/user-attachments/assets/21a6506b-7ce4-4c44-8341-66faf0154397)



# Тестовые примеры
![image](https://github.com/user-attachments/assets/53a84abd-41bd-4f92-8866-1dcfa657948c)
![image](https://github.com/user-attachments/assets/88cc4139-4e77-4f72-a6d1-0196e8ed63a4)


