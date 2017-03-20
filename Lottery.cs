using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    public class Lottery : INotifyPropertyChanged
    {
        public Lottery() { }

        private int[] ballList = new int[7];

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        private int? id;

        public int? Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        private int? no;

        public int? No
        {
            get { return no; }
            set
            {
                if (no != value)
                {
                    no = value;
                    OnPropertyChanged("No");
                }
            }
        }

        public int this[int index]
        {
            get
            {
                return ballList[index];
            }
            set
            {
                if (index >= 0 && index < 7)
                {
                    ballList[index] = value;
                    switch (index)
                    {
                        case 0:
                            this.Redball1 = value;
                            break;
                        case 1:
                            this.Redball2 = value;
                            break;
                        case 2:
                            this.Redball3 = value;
                            break;
                        case 3:
                            this.Redball4 = value;
                            break;
                        case 4:
                            this.Redball5 = value;
                            break;
                        case 5:
                            this.Blueball1 = value;
                            break;
                        case 6:
                            this.Blueball2 = value;
                            break;
                    }
                }

            }
        }

        private int redball1;

        public int Redball1
        {
            get { return redball1; }
            set
            {
                if (redball1 != value)
                {
                    redball1 = value;
                    OnPropertyChanged("Redball1");
                }
            }
        }
        private int redball2;

        public int Redball2
        {
            get { return redball2; }
            set
            {
                if (redball2 != value)
                {
                    redball2 = value;
                    OnPropertyChanged("Redball2");
                }
            }
        }
        private int redball3;

        public int Redball3
        {
            get { return redball3; }
            set
            {
                if (redball3 != value)
                {
                    redball3 = value;
                    OnPropertyChanged("Redball3");
                }
            }
        }
        private int redball4;

        public int Redball4
        {
            get { return redball4; }
            set
            {
                if (redball4 != value)
                {
                    redball4 = value;
                    OnPropertyChanged("Redball4");
                }
            }
        }
        private int redball5;

        public int Redball5
        {
            get { return redball5; }
            set
            {
                if (redball5 != value)
                {
                    redball5 = value;
                    OnPropertyChanged("Redball5");
                }
            }
        }
        private int blueball1;

        public int Blueball1
        {
            get { return blueball1; }
            set
            {
                if (blueball1 != value)
                {
                    blueball1 = value;
                    OnPropertyChanged("Blueball1");
                }
            }
        }
        private int blueball2;

        public int Blueball2
        {
            get { return blueball2; }
            set
            {
                if (blueball2 != value)
                {
                    blueball2 = value;
                    OnPropertyChanged("Blueball2");
                }
            }
        }

        private String resultString;

        public String ResultString
        {
            get
            {
                if (resultString == null)
                {
                    StringBuilder sb = new StringBuilder();
                    for (var i = 0; i < 7; i++)
                    {
                        if (this[i] < 10)
                        {
                            sb.Append("0").Append(this[i]);
                        }
                        else
                        {
                            sb.Append(this[i]);
                        }
                    }

                    resultString = sb.ToString();

                    return resultString;
                }

                return resultString;
            }
            set
            {
                if (resultString != value)
                {
                    resultString = value;
                    OnPropertyChanged("ResultString");
                }
            }
        }
    }
}
