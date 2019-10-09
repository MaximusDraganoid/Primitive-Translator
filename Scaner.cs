using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class ItemInter
    {
        public string name;
        public int value;

        public ItemInter()
        {
            name = "";
            value = 0;
        }
    }

    //структура для хранения области памяти, области команд и области меток
    class Result
    {
        public StackMas<ItemInter> data;
        public StackMas<int> commands;
        public StackMas<int> labels;

        public Result()
        {
            data = new StackMas<ItemInter>(1000);
            commands = new StackMas<int>(1000);
            labels = new StackMas<int>(1000);
        }
    }

    class Scaner
    {
        //таблица разделителей
        public Table TableDelim;
        //таблица ключевых слов
        public Table TableKey;
        //таблица идентификаторов и целых чисел
        public Table TableId;

        //стек - магазин
        public Stack MP;
        //Каретка для перемещения по строке
        public int kar;

        //структура для трансляции и интерпретации
        public Result rez;  

        //строка, хранящая текст из редактора кода
        public string Str;

        //Типы атрибутов = входные символы
        //идентификатор
        public static int ident = 1;
        //константа
        public static int konst = 2;
        //условие if then else 
        public static int condition1 = 3;
        public static int condition2 = 4;
        public static int condition3 = 5;
        //цикл while end_while
        public static int cycle1 = 6;
        public static int cycle2 = 7;
        //арифметические операции + * ^
        public static int arifm1 = 8;
        public static int arifm2 = 9;
        public static int arifm3 = 10;
        //логические операции = #
        public static int logic1 = 11;
        public static int logic2 = 12;
        //разделители ; ( )
        public static int delim1 = 13;
        public static int delim2 = 14;
        public static int delim3 = 15;
        //public static int consc = 0;

        //Магазинные символы
        public static int programm = 16; //программа
        public static int complexop = 17; //совокупность операторов
        public static int operate = 18; //оператор
        public static int endif = 19; //endif
        public static int E = 20; //арифметическое выражение
        public static int Elist = 21; //Е-список
        public static int T = 22; //Т
        public static int Tlist = 23; //Т-список
        public static int F = 24; //F
        public static int P = 25; //P
        public static int R = 26; //R
        public static int logicv = 27; //логическое выражение
        public static int Llist = 28; //Llist
        public static int marker = 0; //маркер дна

        //команды для интерпретатора
        public static int slog = -1;
        public static int umnog = -2;
        public static int steptn = -3;
        public static int lalbe = -4;
        public static int uslovn_po_0 = -5;
        public static int bezuslovn = -6;
        public static int ravno = -7;
        public static int ne_ravno = -8;
        public static int po_sravn = -9;
        public static int prisvoit = -10;



        //конструктор
        public Scaner(){
            TableDelim = new Table(9);
            TableKey = new Table(5);
            TableId = new Table(100);
            //CountId = 0;
            Str = null;
            MP = new Stack();
            rez = new Result();

            //заполнение таблицы разделителей
            TableDelim.AddToTable("+", arifm1);
            TableDelim.AddToTable("*", arifm2);
            TableDelim.AddToTable("^", arifm3);
            TableDelim.AddToTable("=", logic1);
            TableDelim.AddToTable("#", logic2);
            TableDelim.AddToTable(";", delim1);
            TableDelim.AddToTable("(", delim2);
            TableDelim.AddToTable(")", delim3);
            TableDelim.AddToTable(" ", marker);

            //заполнение таблицы ключевых слов
            TableKey.AddToTable("if", condition1);
            TableKey.AddToTable("then", condition2);
            TableKey.AddToTable("else", condition3);
            TableKey.AddToTable("while", cycle1);
            TableKey.AddToTable("end_while", cycle2);
        }
        //проверка на преналежность символа алфавиту
        public bool Affiliation(char symbol)
        {
            if (symbol >= 'a' && symbol <= 'z')
                return true;
            else if (symbol >= '0' && symbol <= '9')
                return true;
            else if (symbol == '(')
                return true;
            else if (symbol == ')')
                return true;
            else if (symbol == '+')
                return true;
            else if (symbol == '*')
                return true;
            else if (symbol == ';')
                return true;
            else if (symbol == '=')
                return true;
            else if (symbol == '#')
                return true;
            else if (symbol == '^')
                return true;
            else if (symbol == '_')
                return true;
            else if (symbol == ' ')
                return true;
            else if (symbol == '\r' || symbol == '\n')
                return true;
            return false;
        }

        //Проверка на разделительный символ
        public bool Delimiter(char symbol)
        {
            if (symbol == '(')
                return true;
            else if (symbol == ')')
                return true;
            else if (symbol == '+')
                return true;
            else if (symbol == '*')
                return true;
            else if (symbol == ';')
                return true;
            else if (symbol == '=')
                return true;
            else if (symbol == '#')
                return true;
            else if (symbol == '^')
                return true;
            else if (symbol == '_')
                return true;
            else if (symbol == ':')
                return true;
            return false;
        }

        //Сканирование строки с возвращением атрибута лексемы. Если лексема не выделена, то вернуть -1. Если конец строки, то вернуть 0
        public Node ScanStr(string str, ref int kar)
        {
            char cur = ' ';
            string num = null;
            string word = null;
            string separator = null;

            if (kar < str.Length)
                cur = str[kar];
            else if (kar > str.Length)
                return null;
            else if (kar == str.Length)
                return TableDelim.SearchByName(" ");

            //пропуск пробелов, табуляции и переход на новую строку
            while ((cur == ' ' || cur == '\t' || cur == '\r' || cur == '\n') && kar < str.Length)
            {
                kar++;
                if (kar < str.Length)
                    cur = str[kar];
                if (kar == str.Length)
                    return TableDelim.SearchByName(" ");
            }
            //проверка на число
            if (cur >= '0' && cur <= '9')
            {
                do
                {
                    num += cur;
                    kar++;
                    if (kar >= str.Length)
                        break;
                    cur = str[kar];
                } while (cur >= '0' && cur <= '9');
                if (cur >= 'a' && cur <= 'z')
                    return null;
                if (TableId.SearchByName_atr(num) == -1)
                    TableId.AddToTable(num, konst);
                return TableId.SearchByName(num);
            }
            //проверка на идентификатор или ключевое слово
            if (cur >= 'a' && cur <= 'z')
            {
                do
                {
                    word += cur;
                    /*kar++;
                    if (kar >= str.Length)
                        break;*/
                    if (kar < str.Length)
                        kar++;
                    else
                        break;
                    cur = str[kar];
                } while (cur >= 'a' && cur <= 'z' || cur == '_');
                if (!Affiliation(cur))
                    return null;
                if (word == TableKey.SearchByName1(word))
                    return TableKey.SearchByName(word);
                else
                {
                    if (TableId.SearchByName_atr(word) == -1)
                        TableId.AddToTable(word, ident);
                    return TableId.SearchByName(word);
                }
            }
            //проверка на разделитель
            if (Delimiter(cur))
            {
                do
                {
                    separator += cur;
                    kar++;
                    if (kar < str.Length)
                        cur = str[kar];
                    else
                        break;
                } while (Delimiter(cur));
                if (!Affiliation(cur))
                    return null;
                return TableDelim.SearchByName(separator);
            }
            else
                return null;
        }

        //МП-автомат
        //msymbol - магазинный символ
        //lexatribut - атрибут считанной лексемы
        public bool MPauto(int msymbol, Node lexatribut, ref Result rez){
            bool exit = true; //в случае допустимой цепочки true, недопустимой false
            int temp; //для смены магазинного символа при вытолкнуть
            Node lex; //для выделения новой лексемы при сдвиге
            int ptr;
            bool flag = false; //для добавления в область данных
            ItemInter item; //для добавления в область данных
            int tempmas;
            switch (msymbol){
                // then
                case 4:
                    switch (lexatribut.Atribute)
                    {
                        // then
                        case 4:
                            MP.Pop();
                            lex = ScanStr(Str, ref kar);
                            temp = MP.Top();
                            exit = MPauto(temp, lex, ref rez);
                            break;
                        default:
                            exit = false;
                            break;
                    }
                    break;
                //end_while
                case 7:
                    switch (lexatribut.Atribute)
                    {
                        //end_while
                        case 7:
                            MP.Pop();
                            lex = ScanStr(Str, ref kar);
                            temp = MP.Top();
                            exit = MPauto(temp, lex, ref rez);
                            break;
                        default:
                            exit = false;
                            break;
                    }
                    break;
                // = 
                case 11:
                    switch (lexatribut.Atribute)
                    {
                        // = 
                        case 11:
                            MP.Pop();
                            lex = ScanStr(Str, ref kar);
                            temp = MP.Top();
                            exit = MPauto(temp, lex, ref rez);
                            break;
                        default:
                            exit = false;
                            break;
                    }
                    break;
                // ;
                case 13:
                    switch (lexatribut.Atribute)
                    {
                        // ;
                        case 13:
                            MP.Pop();
                            lex = ScanStr(Str, ref kar);
                            temp = MP.Top();
                            exit = MPauto(temp, lex, ref rez);
                            break;
                        default:
                            exit = false;
                            break;
                    }
                    break;
                // (
                case 14:
                    switch (lexatribut.Atribute)
                    {
                        // (
                        case 14:
                            MP.Pop();
                            lex = ScanStr(Str, ref kar);
                            temp = MP.Top();
                            exit = MPauto(temp, lex, ref rez);
                            break;
                        default:
                            exit = false;
                            break;
                    }
                    break;
                // )
                case 15:
                    switch (lexatribut.Atribute)
                    {
                        // )
                        case 15:
                            MP.Pop();
                            lex = ScanStr(Str, ref kar);
                            temp = MP.Top();
                            exit = MPauto(temp, lex, ref rez);
                            break;
                        default:
                            exit = false;
                            break;
                    }
                    break;
                // Программа
                case 16:
                    switch (lexatribut.Atribute)
                    {
                        //идентификатор или if или while
                        case 1:
                        case 3:
                        case 6:
                            MP.Pop();
                            MP.Push(complexop);
                            MP.Push(operate);
                            exit = MPauto(operate, lexatribut, ref rez);
                            break;
                        default:
                            exit = false;
                            break;
                    }
                    break;
                //совокупность операторов
                case 17://а здесь нужно что то изменять???
                    switch (lexatribut.Atribute)
                    {
                        //идентификатор или if или while
                        case 1:
                        case 3:
                        case 6:
                            MP.Pop();
                            MP.Push(complexop);
                            MP.Push(operate);
                            exit = MPauto(operate, lexatribut, ref rez);
                            break;
                        // end_while или концевой маркер
                        case 7:
                        case 0:
                            MP.Pop();
                            temp = MP.Top();
                            exit = MPauto(temp, lexatribut, ref rez);
                            break;
                        default:
                            exit = false;
                            break;
                    }
                    break;
                // оператор
                case 18:
                    switch (lexatribut.Atribute)
                    {
                        //оператор присваивания
                        case 1:
                            MP.Pop();
                            MP.Push(delim1);

                            MP.Push(-1); //q2
                            //проверка на наличие идентификатора в области данных
                            for (ptr = 0; ptr < rez.data.Size(); ++ptr)
                            {
                                if (String.Compare(lexatribut.Name, rez.data.Mas[ptr].name) == 0)
                                {
                                    flag = true;
                                    break;
                                }
                            }
                            if (!flag)
                            {
                                item = new ItemInter();
                                item.name = lexatribut.Name;
                                rez.data.Push(item);
                            }
                            MP.Push(ptr);
                            MP.Push(prisvoit);
                            MP.Push(MP.Size() - 3);
                            MP.Push(E);
                            MP.Push(logic1);
                            lex = ScanStr(Str, ref kar);
                            exit = MPauto(logic1, lex, ref rez);
                            break;
                        //условный оператор
                        case 3:
                            MP.Pop();
                            rez.labels.Push(-1);
                            MP.Push(rez.labels.Size() - 1); //z2
                            MP.Push(endif);
                            MP.Push(operate);
                            MP.Push(condition2);
                            MP.Push(rez.labels.Size() - 1); //z1
                            MP.Push(-1);
                            MP.Push(uslovn_po_0);
                            MP.Push(delim3);
                            MP.Push(MP.Size() - 3);
                            MP.Push(logicv);
                            MP.Push(delim2);
                            temp = MP.Top();
                            lex = ScanStr(Str, ref kar);
                            exit = MPauto(delim2, lex, ref rez);
                            break;
                        //оператор цикла
                        case 6:
                            MP.Pop();
                            MP.Push(cycle2);
                            rez.labels.Push(-1);
                            MP.Push(rez.labels.Size() - 1); //w2
                            MP.Push(lalbe);
                            rez.labels.Push(-1); 
                            MP.Push(rez.labels.Size() - 1); //z2
                            MP.Push(bezuslovn);
                            MP.Push(operate);
                            MP.Push(rez.labels.Size() - 2); //w1
                            MP.Push(-1); //p2
                            MP.Push(uslovn_po_0);
                            MP.Push(delim3);
                            MP.Push(MP.Size() - 3); //p1
                            MP.Push(logicv);
                            MP.Push(delim2);
                            MP.Push(rez.labels.Size() - 1); //z2
                            MP.Push(lalbe);
                            lex = ScanStr(Str, ref kar);
                            exit = MPauto(lalbe, lex, ref rez);
                            break;
                        default:
                            exit = false;
                            break;
                    }
                    break;
                //endif
                case 19:
                    switch (lexatribut.Atribute)
                    {
                        //else
                        case 5:
                            MP.Pop(); // выталкиваем endif
                            ptr = MP.Pop(); //указатель из z1
                            rez.labels.Push(-1);
                            MP.Push(rez.labels.Size() - 1); //w2
                            MP.Push(lalbe);
                            MP.Push(operate);
                            MP.Push(ptr); //z2
                            MP.Push(lalbe);
                            MP.Push(rez.labels.Size() - 1); //w1
                            MP.Push(bezuslovn);
                            temp = MP.Top();
                            lex = ScanStr(Str, ref kar);
                            exit = MPauto(temp, lex, ref rez);
                            break;
                        //идентификатор или if или while или концевой маркер
                        case 1:
                        case 3:
                        case 6:
                        case 0:
                            MP.Pop();
                            MP.Push(lalbe);
                            temp = MP.Top();
                            exit = MPauto(temp, lexatribut, ref rez);
                            break;
                        default:
                            exit = false; 
                            break;
                    }
                    break;
                // E
                case 20:
                    switch (lexatribut.Atribute)
                    {
                        //идентификатор или целое
                        case 1:
                        case 2:
                            MP.Pop();
                            MP.Push(-1);
                            MP.Push(Elist);
                            MP.Push(MP.Size() - 2);
                            MP.Push(T);
                            temp = MP.Top();
                            exit = MPauto(T, lexatribut, ref rez);
                            break;
                        default:
                            exit = false;
                            break;
                    }
                    break;
                //E-список
                case 21:
                    switch (lexatribut.Atribute)
                    {
                        // +
                        case 8:
                            MP.Pop();
                            ptr = MP.Pop(); //p1
                                            //новая ячейка
                            item = new ItemInter();
                            rez.data.Push(item);
                            MP.Push(rez.data.Size() - 1); //r2
                            MP.Push(Elist);
                            MP.Push(rez.data.Size() - 1);//r1
                            MP.Push(-1); //q2
                            MP.Push(ptr); //p2
                            MP.Push(slog); //действие сложить
                            MP.Push(MP.Size() - 3); //q1
                            MP.Push(T);
                            temp = MP.Top();
                            lex = ScanStr(Str, ref kar);
                            exit = MPauto(T, lex, ref rez);
                            break;
                        // ;
                        case 13:
                            MP.Pop();
                            ptr = MP.Pop(); //p1
                            tempmas = MP.Pop(); //t2
                            MP.PushIndex(tempmas, ptr);
                            temp = MP.Top();
                            exit = MPauto(temp, lexatribut, ref rez);
                            break;
                        default:
                            exit = false;
                            break;

                    }
                    break;
                //Т
                case 22:
                    switch (lexatribut.Atribute)
                    {
                        //идентификатор или целое
                        case 1:
                        case 2:
                            MP.Pop();
                            MP.Push(-1);
                            MP.Push(Tlist);
                            MP.Push(MP.Size() - 2);
                            MP.Push(F);
                            temp = MP.Top();
                            exit = MPauto(F, lexatribut, ref rez);
                            break;
                        default:
                            exit = false;
                            break;
                    }
                    break;
                //Т-список
                case 23:
                    switch (lexatribut.Atribute)
                    {
                        // *
                        case 9:
                            MP.Pop();
                            ptr = MP.Pop(); //p1
                                            //новая ячейка
                            item = new ItemInter();
                            rez.data.Push(item);
                            MP.Push(rez.data.Size() - 1); //r2
                            MP.Push(Tlist);
                            MP.Push(rez.data.Size() - 1); //r1
                            MP.Push(-1); //q2
                            MP.Push(ptr); //p2
                            MP.Push(umnog); //действие умножить
                            MP.Push(MP.Size() - 3); //q1
                            MP.Push(F);
                            temp = MP.Top();
                            lex = ScanStr(Str, ref kar);
                            exit = MPauto(F, lex, ref rez);
                            break;
                        // + или ;
                        case 8:
                        case 13:
                            MP.Pop();
                            ptr = MP.Pop(); //p1
                            tempmas = MP.Pop(); //t2
                            MP.PushIndex(tempmas, ptr);
                            temp = MP.Top();
                            exit = MPauto(temp, lexatribut, ref rez);
                            break;
                        default:
                            exit = false;
                            break;
                    }
                    break;
                //F
                case 24:
                    switch (lexatribut.Atribute)
                    {
                        //идентификатор или целое
                        case 1:
                        case 2:
                            MP.Pop();
                            MP.Push(-1);
                            MP.Push(R);
                            MP.Push(MP.Size() - 2);
                            MP.Push(P);
                            exit = MPauto(P, lexatribut, ref rez); 
                            break;
                        default:
                            exit = false;
                            break;
                    }
                    break;
                //P
                case 25:
                    switch (lexatribut.Atribute)
                    {
                        //идентификатор или целое 
                        case 1:
                        case 2:
                            MP.Pop();
                            temp = MP.Top();
                            //проверка на наличие идентификатора или числа в области данных
                            for (ptr = 0; ptr < rez.data.Size(); ++ptr)
                            {
                                if (String.Compare(lexatribut.Name, rez.data.Mas[ptr].name) == 0)
                                {
                                    flag = true;
                                    break;
                                }
                            }
                            if (!flag)
                            {
                                item = new ItemInter();
                                item.name = lexatribut.Name;
                                if (lexatribut.Atribute == 2)
                                    item.value = Convert.ToInt32(lexatribut.Name);
                                rez.data.Push(item);
                            }
                            tempmas = MP.Pop(); //адрес из ячейки p2
                            MP.PushIndex(tempmas, ptr);
                            temp = MP.Top();
                            lex = ScanStr(Str, ref kar);
                            exit = MPauto(temp, lex, ref rez);
                            break;
                        default:
                            exit = false;
                            break;
                    }
                    break;
                //R
                case 26:
                    switch (lexatribut.Atribute)
                    {
                        // + или * или ;
                        case 8:
                        case 9:
                        case 13:
                            MP.Pop();
                            ptr = MP.Pop(); //p1
                            tempmas = MP.Pop(); //t2
                            MP.PushIndex(tempmas, ptr);
                            temp = MP.Top();
                            exit = MPauto(temp, lexatribut, ref rez);
                            break;
                        // ^
                        case 10:
                            MP.Pop();
                            ptr = MP.Pop();
                            //выделяем память под результат возведения в степень
                            item = new ItemInter();
                            rez.data.Push(item);
                            MP.Push(rez.data.Size() - 1); //r2
                            MP.Push(R);
                            MP.Push(rez.data.Size() - 1); //r1
                            MP.Push(-1); //q2
                            MP.Push(ptr); //p2
                            MP.Push(steptn);
                            MP.Push(MP.Size() - 3);
                            MP.Push(P);
                            lex = ScanStr(Str, ref kar);
                            exit = MPauto(P, lex, ref rez);
                            break;
                        default:
                            exit = false;
                            break;
                    }
                    break;
                //логическое выражение
                case 27:
                    switch (lexatribut.Atribute)
                    {
                        //идентификатор или целое
                        case 1:
                        case 2:
                            MP.Pop();
                            MP.Push(-1);
                            MP.Push(Llist);
                            MP.Push(MP.Size() - 2);
                            MP.Push(P);
                            //lex = ScanStr(Str, ref kar);
                            exit = MPauto(P, lexatribut, ref rez);
                            break;
                        default:
                            exit = false;
                            break;
                    }
                    break;
                //логический оператор
                case 28:
                    switch (lexatribut.Atribute)
                    {
                        // = или #
                        case 11:
                        case 12:
                            MP.Pop();
                            ptr = MP.Pop(); //p1
                            item = new ItemInter();
                            rez.data.Push(item);
                            tempmas = MP.Pop();
                            MP.PushIndex(tempmas, rez.data.Size() - 1); //r1
                            MP.Push(rez.data.Size() - 1);
                            MP.Push(-1); //q2
                            MP.Push(ptr); //p2
                            if (lexatribut.Atribute == logic1)
                                MP.Push(ravno); // равно
                            else
                                MP.Push(ne_ravno); //не равно
                            MP.Push(MP.Size() - 3);
                            MP.Push(P);
                            temp = MP.Top();
                            lex = ScanStr(Str, ref kar);
                            exit = MPauto(temp, lex, ref rez);
                            break;
                        default:
                            exit = false;
                            break;
                    }
                    break;
                //маркер дна
                case 0:
                    switch (lexatribut.Atribute)
                    {
                        // концевой маркер
                        case 0:
                            MP.Pop();
                            exit = true;
                            break;
                        default:
                            exit = false;
                            break;
                    }
                    break;
                //присвоить
                case -10:
                    rez.commands.Push(MP.Pop());
                    for (int i = 0; i < 2; ++i){
                        rez.commands.Push(MP.Pop());
                    }
                    temp = MP.Top();
                    exit = MPauto(temp, lexatribut, ref rez);
                    break;
                //сложить
                case -1:
                    rez.commands.Push(MP.Pop());
                    for (int i = 0; i < 3; ++i)
                        rez.commands.Push(MP.Pop());
                    temp = MP.Top();
                    exit = MPauto(temp, lexatribut, ref rez);
                    break;
                //умножить
                case -2:
                    rez.commands.Push(MP.Pop());
                    for (int i = 0; i < 3; ++i)
                        rez.commands.Push(MP.Pop());
                    temp = MP.Top();
                    exit = MPauto(temp, lexatribut, ref rez);
                    break;
                //возведение в степень
                case -3:
                    rez.commands.Push(MP.Pop());
                    for (int i = 0; i < 3; ++i)
                        rez.commands.Push(MP.Pop());
                    temp = MP.Top();
                    exit = MPauto(temp, lexatribut, ref rez);
                    break;
                //метка
                case -4:
                    MP.Pop(); //вытолкнули метку
                    tempmas = MP.Pop(); // атрибут метки
                    rez.labels.Mas[tempmas] = rez.commands.Size();
                    temp = MP.Top();
                    exit = MPauto(temp, lexatribut, ref rez);
                    break;
                //условный переход по нулю
                case -5:
                    rez.commands.Push(MP.Pop());
                    for (int i = 0; i < 2; ++i)
                        rez.commands.Push(MP.Pop());
                    temp = MP.Top();
                    exit = MPauto(temp, lexatribut, ref rez);
                    break;
                //безусловный переход
                case -6:
                    rez.commands.Push(MP.Pop());
                    rez.commands.Push(MP.Pop());
                    temp = MP.Top();
                    exit = MPauto(temp, lexatribut, ref rez);
                    break;
                //условный переход по сравнению
                case -9:
                    rez.commands.Push(MP.Pop());
                    for (int i = 0; i < 2; ++i)
                        rez.commands.Push(MP.Pop());
                    temp = MP.Top();
                    exit = MPauto(temp, lexatribut, ref rez);
                    break;
                //равно
                case -7:
                    rez.commands.Push(MP.Pop());
                    for (int i = 0; i < 3; ++i)
                        rez.commands.Push(MP.Pop());
                    temp = MP.Top();
                    exit = MPauto(temp, lexatribut, ref rez);
                    break;
                //не равно
                case -8:
                    rez.commands.Push(MP.Pop());
                    for (int i = 0; i < 3; ++i)
                        rez.commands.Push(MP.Pop());
                    temp = MP.Top();
                    exit = MPauto(temp, lexatribut, ref rez);
                    break;
                default:
                    exit = false;
                    break;
            }
            return exit;
        }

        public void Interpretator(ref Result rez){
            int i = 0;
            int elem = rez.commands.Mas[0];
            while (i < rez.commands.Size())
            {
                //int elem = rez.commands.Mas[i];
                switch (elem)
                {
                    case -10:
                        rez.data.Mas[rez.commands.Mas[i + 1]].value = rez.data.Mas[rez.commands.Mas[i + 2]].value;
                        i += 3;
                        elem = rez.commands.Mas[i];
                        break;
                    case -1:
                        rez.data.Mas[rez.commands.Mas[i + 3]].value = rez.data.Mas[rez.commands.Mas[i + 1]].value + rez.data.Mas[rez.commands.Mas[i + 2]].value;
                        i += 4;
                        elem = rez.commands.Mas[i];
                        break;
                    case -2:
                        rez.data.Mas[rez.commands.Mas[i + 3]].value = rez.data.Mas[rez.commands.Mas[i + 1]].value * rez.data.Mas[rez.commands.Mas[i + 2]].value;
                        i += 4;
                        elem = rez.commands.Mas[i];
                        break;
                    case -7:
                        if (rez.data.Mas[rez.commands.Mas[i + 1]].value == rez.data.Mas[rez.commands.Mas[i + 2]].value)
                            rez.data.Mas[rez.commands.Mas[i + 3]].value = 1;
                        else
                            rez.data.Mas[rez.commands.Mas[i + 3]].value = 0;
                        i += 4;
                        elem = rez.commands.Mas[i];
                        break;
                    case -8:
                        if (rez.data.Mas[rez.commands.Mas[i + 1]].value != rez.data.Mas[rez.commands.Mas[i + 2]].value)
                            rez.data.Mas[rez.commands.Mas[i + 3]].value = 1;
                        else
                            rez.data.Mas[rez.commands.Mas[i + 3]].value = 0;
                        i += 4;
                        elem = rez.commands.Mas[i];
                        break;
                    case -4:
                        i = rez.labels.Mas[rez.commands.Mas[i + 1]];
                        elem = rez.commands.Mas[i];
                        break;
                    case -9:
                        if (rez.data.Mas[rez.commands.Mas[i + 1]].value == 1)
                        {
                            i = rez.labels.Mas[rez.commands.Mas[i + 2]];
                            elem = rez.commands.Mas[i];
                        }
                        else
                        {
                            i += 3;
                            elem = rez.commands.Mas[i];
                        }
                        break;
                    case -5:
                        if (rez.data.Mas[rez.commands.Mas[i + 1]].value == 0)
                        {
                            i = rez.labels.Mas[rez.commands.Mas[i + 2]];
                            elem = rez.commands.Mas[i];
                        }
                        else
                        {
                            i += 3;
                            elem = rez.commands.Mas[i];
                        }
                        break;
                    case -6:
                        i = rez.labels.Mas[rez.commands.Mas[i + 1]];
                        elem = rez.commands.Mas[i];
                        break;
                    case -3:
                        rez.data.Mas[rez.commands.Mas[i + 3]].value = (int)Math.Pow(rez.data.Mas[rez.commands.Mas[i + 1]].value, rez.data.Mas[rez.commands.Mas[i + 2]].value);
                        i += 4;
                        elem = rez.commands.Mas[i];
                        break;
                    default:
                        break;
                }
            }
            return;

        }

    }
}
