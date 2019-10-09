using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    //namespace TableOfNode
    //{

        //включение идентефикатора в таблицу
        //поиск по имени в таблице 
        //исключение описания идентефикатора в таблице 
        //описание идентефикатора состоит из имени и атрибута 
        /*

            способ представления таблицы - массив указателей на динамические элементы. Метод поиска - линейный поиск           
        */

        class Node
        {
            string name;
            int atribut;

            public string Name
            {

                get
                {
                    return name;
                }

                set
                {
                    name = value;
                }
            }

            public int Atribute
            {

                get
                {
                    return atribut;
                }

                set
                {
                    atribut = value;
                }
            }

            public Node()
            {

                name = "";
                atribut = -1;
            }
        }

        class Table
        {
            public Node[] array; // сама таблица ( на основе массива ) 
            public int NumOfIdenteficator; // число идентефикаторов в таблице
            public int[] FreePlace;//индекс последнего свободного места, по этому индексу будет записан вновь прибывший элемент таблицы

            public Table(int num)
            {


                NumOfIdenteficator = num;
                FreePlace = new int[NumOfIdenteficator];
                array = new Node[NumOfIdenteficator];
                FreePlace = new int[NumOfIdenteficator];

                for (int indx = 0; indx < NumOfIdenteficator; indx++)
                {

                    array[indx] = null;
                    FreePlace[indx] = -1;
                }
            }

            public int SearchPlace()
            {

                int fear = -1;

                for (int indx = 0; indx < NumOfIdenteficator; indx++)
                {
                    if (FreePlace[indx] == fear)
                    {
                        return indx;
                    }
                }

                return fear;
            }

            /*public void AddToTable(string str, int atr)
            {

                int check1 = SearchByName(str);
                //int check2 = SearchByAtr(atr);

                if (check1 != -1)
                    atr = Scaner.LexBlcock.error;
                else
                {
                    int newIndx = SearchPlace();
                    if (newIndx != -1)
                    {
                        Node add = new Node();
                        add.Name = str;
                        add.Atribute = atr;
                        array[newIndx] = add;
                        FreePlace[newIndx] = 0;
                        // !!!!!!!!!!!!!!!Переделать этот момент, когда напишу удаление, скорее всего нужна будет
                        // функция, вычиисляющая значение блятбб!!! 
                    }
                    //else Console.WriteLine("Свободного места нет");
                }

                return;
            }*/

            public void AddToTable(string str, int atr)
            {

                /*int check1 = SearchByName(str);
                //int check2 = SearchByAtr(atr);

                if (check1 != -1)
                    atr = Scaner.LexBlcock.error;
                else
                {*/
                    int newIndx = SearchPlace();
                    if (newIndx != -1)
                    {
                        Node add = new Node();
                        add.Name = str;
                        add.Atribute = atr;
                        array[newIndx] = add;
                        FreePlace[newIndx] = 0;
                        // !!!!!!!!!!!!!!!Переделать этот момент, когда напишу удаление, скорее всего нужна будет
                        // функция, вычиисляющая значение блятбб!!! 
                    }
                    //else Console.WriteLine("Свободного места нет");
               // }

                return;
            }


            //при поиске по имени возвращаем атрибут 
            public Node SearchByName(string str)
            {

                //int NumInTable = -1;
                Node temp = null;
                for (int indx = 0; indx < NumOfIdenteficator; indx++)
                {
                    if (array[indx] != null)
                        if (array[indx].Name == str)
                        {
                            //flag = new Node();
                            //flag.Name = array[indx].Name;
                            //flag.Atribute = array[indx].Atribute;
                            temp = array[indx];
                            return temp;
                        }
                }
                return temp;
            }

        public int SearchByName_atr(string str)
        {

            //int NumInTable = -1;
            int temp = -1;
            for (int indx = 0; indx < NumOfIdenteficator; indx++)
            {
                if (array[indx] != null)
                    if (array[indx].Name == str)
                    {
                        //flag = new Node();
                        //flag.Name = array[indx].Name;
                        //flag.Atribute = array[indx].Atribute;
                        temp = array[indx].Atribute;
                        return temp;
                    }
            }
            return temp;
        }


        //при поиске по имени возвращаем идентификатор
        //при поиске по имени возвращаем атрибут 
        public string SearchByName1(string str)
        {

            //int NumInTable = -1;
            string temp = null;
            for (int indx = 0; indx < NumOfIdenteficator; indx++)
            {
                if (array[indx] != null)
                    if (array[indx].Name == str)
                    {
                        //flag = new Node();
                        //flag.Name = array[indx].Name;
                        //flag.Atribute = array[indx].Atribute;
                        temp = array[indx].Name;
                        return temp;
                    }
            }
            return temp;
        }

        public int SearchByNameAtr(string str)
            {

                int NumInTable = -1;
                for (int indx = 0; indx < NumOfIdenteficator; indx++)
                {
                    if (array[indx] != null)
                        if (array[indx].Name == str)
                        {
                            //flag = new Node();
                            //flag.Name = array[indx].Name;
                            //flag.Atribute = array[indx].Atribute;
                            NumInTable = indx;
                            return NumInTable;
                        }
                }
                return NumInTable;
            }

            public int SearchByAtr(int atr)
            {

                int NumInTable = -1;
                for (int indx = 0; indx < NumOfIdenteficator; indx++)
                {
                    if (array[indx] != null)
                        if (array[indx].Atribute == atr)
                        {
                            //flag = new Node();
                            //flag.Name = array[indx].Name;
                            //flag.Atribute = array[indx].Atribute;
                            NumInTable = indx;
                            return NumInTable;
                        }
                }
                return NumInTable;
            }
        /*
            public void DeleteByName(string str)
            {
                //int check = SearchByName(str);

                if (check == -1)
                {
                    Console.WriteLine(" Идентификатор с таким именем отсутствует ");
                }
                else
                {

                    array[check] = null;
                    FreePlace[check] = -1;
                }

                return;
            }
        */
            public void Print()
            {
                Console.WriteLine("=======================================");
                for (int indx = 0; indx < NumOfIdenteficator; indx++)
                {

                    if (FreePlace[indx] == 0)
                    {
                        Console.WriteLine("Имя идентификатора : " + array[indx].Name + " атрибут идентификатора : " + array[indx].Atribute);
                    }

                }
                Console.WriteLine("=======================================");
                return;
            }
        }
    //}
}
