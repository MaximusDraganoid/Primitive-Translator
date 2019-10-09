using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    //элемент стека
    class NodeSt
    {
        //магазинный символ
        public int state;
        public NodeSt next;
        public int number;

        public NodeSt()
        {
            state = 0;
            number = 0;
            next = null;
        }
    }

    //стек для магазинной памяти
    class Stack
    {
        private NodeSt Head;
        public NodeSt Dostup
        {
            get
            {
                return Head;
            }
            set
            {
                Head = value;
            }
        }

        public Stack()
        {
            Head = null;
        }

        public int Size()
        {
            return Dostup.number + 1;
        }

        //добавить в стек
        public void Push(int x)
        {
            if (Head == null)
            {
                NodeSt p = new NodeSt();
                p.state = x;
                p.next = Head;
                Head = p;
            }
            else
            {
                NodeSt p = new NodeSt();
                p.state = x;
                p.number = Head.number + 1;
                p.next = Head;
                Head = p;
            }
        }

        //извлечь из стека
        public int Pop()
        {
            NodeSt p = Head;
            Head = Head.next;
            return p.state;
        }

        //считать верхушку стека
        public int Top()
        {
            int x = Head.state;
            return x;
        }

        public void PushIndex(int index, int token)
        {
            NodeSt temp = Dostup;
            bool flag = false;
            while (!flag && temp != null)
            {
                if (temp.number == index)
                    flag = true;
                else
                    temp = temp.next;
            }
            if (flag)
                temp.state = token;
        }

        //проверка на пустоту - истина, если стек пуст
        public bool IsEmpty()
        {
            if (Head == null)
                return true;
            else
                return false;
        }
    }
}
