using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp2
{
    class ViewModel : ViewModelBase
    {
        private int round;
        private string[] operations = { "+", "-", "*" };
        int min = 0, max = 0, good = 0, bad = 0, temp = 0;
        List<int> Answers = new List<int>();
        public ViewModel()
        {
            Level = new RelayCommand(
                (parameter) =>
                {
                    Task = "Kliknij przycisk aby zacząć";
                    NewGame();
                    Score();
                    switch (parameter.ToString())
                    {
                        case "Easy":
                            min = 1;
                            max = 10;
                            break; 
                        case "Medium":
                            min = 10;
                            max = 70;
                            break; 
                        case "Hard":
                            min = 30;
                            max = 150;
                            break;
                    }
                    RefreshCanExecutes();
                });
            ClickCommand = new RelayCommand(
               (parameter) =>
               {
                   Score();
                   if (round != 10)
                   {
                       int param = Int32.Parse((string)parameter);
                       if (param == Answers.IndexOf(temp))
                       {
                           good++;
                       }
                       else
                       {
                           bad++;
                       }
                       Answers.Clear();
                       round++;
                       int One = Rand(min, max);
                       System.Threading.Thread.Sleep(100);
                       int Two = Rand(min, max);
                       int Op = Rand(0, 2);
                       switch (Op)
                       {
                           case 0:
                               temp = One + Two;
                               Task = One + " + " + Two + " = ?";
                               break;
                           case 1:
                               temp = One - Two;
                               Task = One + " - " + Two + " = ?";
                               break;
                           case 2:
                               temp = One * Two;
                               Task = One + " * " + Two + " = ?";
                               break;
                       }
                       Answers.Add(temp);
                       while (Answers.Count < 4)
                       {
                           int rnd = Rand(temp - 10, temp + 10);
                           if (!Answers.Contains(rnd)) Answers.Add(rnd);
                       }
                       Answers.Sort();
                       int i = 0;
                       foreach (int number in Answers)
                       {
                           Content[i] = number.ToString();
                           i++;
                       }
                   }
                   else
                   {
                       Task = WhoWin();
                   }
                   RefreshCanExecutes();
               });
         }
        private int Rand(int min, int max)
        {
            Random rnd = new Random();
            return rnd.Next(min, max+1);
        }
        private void Score()
        {
            Good = "Dobrze: " + good;
            Bad = "Źle:" + bad;
        }
        private string WhoWin()
        {
            return (good > bad) ?  "Wygrałeś!" : "Przegrałeś!";
        }
        private void NewGame()
        {
            Content = new ObservableCollection<string>(new string[4]);
            good = 0;
            bad = 0;
            round = 0;
        }
        void RefreshCanExecutes()
        {
            ((RelayCommand)ClickCommand).OnCanExecuteChanged();
            ((RelayCommand)Level).OnCanExecuteChanged();
        }
        public string Task
        {
            private set { SetProperty(ref task, value); }
            get { return task; }
        }
        public ObservableCollection<string> Content
        {
            private set { SetProperty(ref content, value); }
            get { return content; }
        }
        public string Good
        {
            private set { SetProperty(ref good1, value); }
            get { return good1; }
        }
        public string Bad
        {
            private set { SetProperty(ref bad1, value); }
            get { return bad1; }
        }
        public ICommand ClickCommand { private set; get; }
        public ICommand Level { private set; get; }
        public string task;
        public string good1;
        public string bad1;
        public ObservableCollection<string> content;
    }
    

}
