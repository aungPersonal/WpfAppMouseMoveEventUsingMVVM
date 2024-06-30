using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Country { get; set; }
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        private string _mousePosition;
        public ObservableCollection<Person> People { get; set; }
        public ICommand MouseMoveCommand { get; private set; }
        public ICommand SortCommand { get; private set; }

        public string MousePosition
        {
            get { return _mousePosition; }
            set
            {
                _mousePosition = value;
                OnPropertyChanged(nameof(MousePosition));
            }
        }

        public MainViewModel()
        {
            People = new ObservableCollection<Person>
        {
            new Person { Name = "John Doe", Age = 30, Country = "USA" },
            new Person { Name = "Anna Smith", Age = 25, Country = "UK" },
            new Person { Name = "Pedro Alonso", Age = 40, Country = "Spain" },
            new Person { Name = "Maria Garcia", Age = 35, Country = "Mexico" }
        };

            SortCommand = new RelayCommand(SortByName);
            MouseMoveCommand = new RelayCommand(HandleMouseMove);
        }

        private void SortByName(object parameter)
        {
            var sortedPeople = new ObservableCollection<Person>(
                People.OrderBy(p => p.Name)
            );

            People.Clear();
            foreach (var person in sortedPeople)
            {
                People.Add(person);
            }
        }

        private void HandleMouseMove(object parameter)
        {
            if (parameter is MouseEventArgs mouseEventArgs)
            {
                var position = mouseEventArgs.GetPosition((IInputElement)mouseEventArgs.Source);
                MousePosition = $"X: {position.X}, Y: {position.Y}";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}