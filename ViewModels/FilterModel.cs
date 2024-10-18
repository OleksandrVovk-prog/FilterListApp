using FilterListApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FilterListApp.ViewModels
{
    public class FilterViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private string _filterText;
        private ObservableCollection<Person> _people;
        private ObservableCollection<Person> _filteredPeople;

        public ObservableCollection<Person> People
        {
            get => _people;
            set
            {
                _people = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Person> FilteredPeople
        {
            get => _filteredPeople;
            set
            {
                _filteredPeople = value;
                OnPropertyChanged();
            }
        }

        public string FilterText
        {
            get => _filterText;
            set
            {
                if (_filterText != value)
                {
                    _filterText = value;
                    OnPropertyChanged();
                    FilterItems();
                }
            }
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(FilterText))
                {
                    if (string.IsNullOrWhiteSpace(FilterText) && FilterText?.Length > 0)
                    {
                        return "Filter text cannot be empty.";
                    }
                    else if (FilterText?.Length > 15)
                    {
                        return "Filter text cannot exceed 15 characters.";
                    }
                }
                return null;
            }
        }

        public FilterViewModel()
        {
            People = new ObservableCollection<Person>();
            LoadData();
            FilterItems();
        }

        private void LoadData()
        {
            // Sample data
            People.Add(new Person { Name = "John Doe", Age = 30, Occupation = "Engineer", Address = "123 Main St" });
            People.Add(new Person { Name = "Jane Smith", Age = 25, Occupation = "Doctor", Address = "456 Oak Ave" });
            People.Add(new Person { Name = "Alice Johnson", Age = 28, Occupation = "Teacher", Address = "789 Pine Rd" });
            People.Add(new Person { Name = "Bob Brown", Age = 35, Occupation = "Artist", Address = "101 Maple Blvd" });
            People.Add(new Person { Name = "Charlie Black", Age = 25, Occupation = "Lawyer", Address = "202 Elm St" });
        }

        private void FilterItems()
        {
            if (string.IsNullOrWhiteSpace(FilterText))
            {
                FilteredPeople = new ObservableCollection<Person>(People);
            }
            else
            {
                string filter = FilterText.ToLowerInvariant();

                var filtered = People.Where(p =>
                {
                    string name = p.Name?.ToLowerInvariant() ?? "";
                    string age = p.Age.ToString();
                    string occupation = p.Occupation?.ToLowerInvariant() ?? "";
                    string address = p.Address?.ToLowerInvariant() ?? "";

                    string allProperties = $"{name} {age} {occupation} {address}";

                    return allProperties.Contains(filter);
                });

                FilteredPeople = new ObservableCollection<Person>(filtered);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}