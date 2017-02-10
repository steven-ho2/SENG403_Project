using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Team4Clock
{
    /// <summary>
    /// Interaction logic for ListOfAlarms.xaml
    /// </summary>
    public partial class ListOfAlarms : UserControl
    {
        private MainWindow mw = new MainWindow();
        private List<DateTime> alarmList;
       
        public ListOfAlarms()
        {
            InitializeComponent();
            

        }

        public ListOfAlarms(MainWindow mw, List<DateTime> list)
        {
            
            this.mw = mw;
            this.alarmList = list;
            Console.WriteLine(list.Count);
            
            InitializeComponent();
            addAlarms();
        }

        private void List_Back_Click(object sender, RoutedEventArgs e)
        {
            (this.Parent as Panel).Children.Remove(this);

        }

        private void addAlarms()
        {
            foreach(DateTime i in alarmList)
            {
                AlarmUI alarm = new AlarmUI(i);  
               this.listStack.Children.Add(alarm);
            }
        }
        
    }
}
