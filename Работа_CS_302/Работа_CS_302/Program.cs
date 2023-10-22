using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Работа_CS_302
{
    delegate void dlgChange1();

    // Событие с объектом события
    delegate void dlgChange2(object sender);

    // Событие с параметром
    delegate void dlgChange3(object sender, int x, int y);

    // Стандартное событие с параметрами
    delegate void dlgChange4(object sender, PointEventArgs e);

    // Класс, содержащий параметры для события
    class PointEventArgs : EventArgs
    {
        public int x; public int y;

        public PointEventArgs(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    delegate void SampleEventHandler (string s);

    internal class Program
    {
        // Обработчик для события Change1
        static void Handler1()
        {
            Console.WriteLine("Change1");
        }

        // Обработчики для события Change2
        static void Handler2a(object o)
        {
            Console.WriteLine("Change2a");
        }

        static void Handler2b(object o)
        {
            Console.WriteLine("Change2b" + " to (" + ((Point)o).x + ")");
        }

        // Обработчики для события Change3
        static void Handler3(object o, int x, int y)
        {
            Console.WriteLine("Change3" + " to (" + x + "," + y + ")");
        }

        // Обработчик для события Change4
        static void Handler4(object sender, PointEventArgs e)
        {
            Console.WriteLine("Change4" + " to (" + e.x + "," + e.y + ")");
        }

        // Обработчик для стандартного события Change
        static void Handler(object sender, EventArgs e)
        {
            Console.WriteLine("Change - empty EventArgs");
        }

        static void Handler2(string s)
        {
            Console.WriteLine(s);
        }

        class Point
        {
            public int x = 0;
            public int y = 0;

            public Point () { }

            public Point (int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public void SetValues(int x, int y)
            {
                this.x = x;
                this.y = y;

                if (Change1 != null)
                {
                    Change1();
                }

                if (Change2 != null)
                {
                    Change2((object)this);
                }

                if (Change3 != null)
                {
                    Change3((object)this, x, y);
                }

                if (Change4 != null)
                {
                    Change4((object)this, new PointEventArgs(x, y));
                }

                if (Change != null)
                {
                    // Есть 2 способа передачи пустых параметров
                    Change((object)this, EventArgs.Empty);
                    Change((object)this, new EventArgs());
                }

                if (Change5 != null)
                {
                    Change5 ((object)this, new PointEventArgs(x,y));
                }
            }

            public event dlgChange1 Change1;

            public event dlgChange2 Change2;

            public event dlgChange3 Change3;

            public event dlgChange4 Change4;

            // Событие с использованием стандартного делегата (без параметров)
            public event EventHandler Change;

            // Событие с использованием стандартного делегата (с параметрами)
            public event EventHandler<PointEventArgs> Change5;

        }

        //////////////////////////////////

        class ArrayWithEvents : ArrayList
        {
            public event EventHandler Changed;

            protected virtual void OnChanged(EventArgs e)
            {
                if (Changed != null)
                {
                    Changed(this, e);
                }
            }

            public override int Add(object value)
            {
                int i = base.Add(value);
                OnChanged(EventArgs.Empty);
                return i;
            }

            public override void Clear()
            {
                base.Clear();
                OnChanged(EventArgs.Empty);
            }

            public override object this[int index]
            {
                set
                {
                    base[index] = value; OnChanged(EventArgs.Empty);
                }
            }
        }

        class ListEventListener
        {
            private ArrayWithEvents list;

            private void ListenChanged(object sender, EventArgs e)
            {
                Console.WriteLine("Event fired.");
            }

            public ListEventListener(ArrayWithEvents list)
            {
                this.list = list;
                this.list.Changed += new EventHandler(ListenChanged);
            }

            public void Detach()
            {
                this.list.Changed -= new EventHandler(ListenChanged);
                this.list = null;
            }
        }

        interface IEventInterface
        {
            event EventHandler Changed;

            void FireEvent(EventArgs e);
        }

        class TestEventInterface : IEventInterface
        {
            public event EventHandler Changed;

            public void FireEvent(EventArgs e)
            {
                EventHandler t = Changed;

                if (t != null)
                {
                    t((object)this, e);
                }
            }

            public void ChangeData()
            {
                FireEvent(EventArgs.Empty);
            }
        }

        interface IEvents1
        {
            event EventHandler SampleEvent;
        }

        interface IEvents2
        {
            event SampleEventHandler SampleEvent;
        }

        class TestTwoEvents : IEvents1, IEvents2
        {
            public event EventHandler SampleEvent;

            private SampleEventHandler SampleEventStorage;

            event SampleEventHandler IEvents2.SampleEvent
            {
                add { SampleEventStorage += value; }
                remove { SampleEventStorage -= value; }
            }

            public void FireEvents()
            {
                if (SampleEvent != null)
                {
                    SampleEvent((object)this, EventArgs.Empty);
                }

                if (SampleEventStorage != null)
                {
                    SampleEventStorage("SampleEventStorage fired");
                }
            }
        }


        static void Main(string[] args)
        {
            //Point a = new Point(1,1);

            //// Свзяываем делегат и событие
            //a.Change1 += new dlgChange1(Handler1);

            //a.SetValues(2, 2);

            //a.Change1 -= new dlgChange1(Handler1);

            //a.SetValues(1, 1);

            //a.Change2 += new dlgChange2(Handler2a);
            //a.Change2 += new dlgChange2(Handler2b);

            //a.SetValues(3,3);

            //a.Change2 -= new dlgChange2(Handler2a);
            //a.Change2 -= new dlgChange2(Handler2b);

            //a.Change3 += new dlgChange3(Handler3);

            //a.SetValues(5,5);

            //a.Change3 -= new dlgChange3(Handler3);

            ////////////////////////////////////

            //a.Change4 += new dlgChange4(Handler4);

            //a.SetValues(6,6);

            //a.Change4 -= new dlgChange4(Handler4);

            //a.Change += new EventHandler(Handler);

            //a.SetValues(7, 7);

            //a.Change -= new EventHandler(Handler);

            //a.Change5 += new EventHandler<PointEventArgs>(Handler4);

            //a.SetValues(8,8);

            //////////////////////////////////// Классы как приемники событий
            
            //ArrayWithEvents list = new ArrayWithEvents();

            //ListEventListener listener = new ListEventListener(list);

            //list.Add("abc");

            //list.Add(123);

            //list.Clear();

            //listener.Detach();

            //////////////////////////////////// Наследование интерфейсов с событием

            //IEventInterface IE = new TestEventInterface();

            //IE.Changed += new EventHandler(Handler);

            //IE.FireEvent(EventArgs.Empty);
            //((TestEventInterface)IE).ChangeData();

            //////////////////////////////////// Наследование двух интерфейсов с событием

            TestTwoEvents test = new TestTwoEvents();

            EventHandler temp = new EventHandler(Handler);
            ((IEvents1)test).SampleEvent += temp;
            test.FireEvents();

            SampleEventHandler temp2 = new SampleEventHandler(Handler2);
            ((IEvents2)test).SampleEvent += temp2;
            test.FireEvents();
        }
    }
}
