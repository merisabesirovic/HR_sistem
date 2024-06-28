using HR_sistem.Models;
using HR_sistem.Repositories;
using System;
using System.Windows;
using System.Windows.Input;
//view model za dodavnje zaposlenog 
namespace HR_sistem.ViewModels
{

    public class AddEmployeeViewModel : ViewModelBase
    {
        public ICommand AddUserCommand { get; set; }

        public RadnikHR CurrentUserAccount { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
        public string Position { get; set; }
        public int WorkingHours { get; set; }
        public float PayingHour { get; set; }
        public DateTime? DateOfStart { get; set; }
        public Odeljenje Odeljenje { get; set; }

        private readonly AddEmployee _addUserRepository;
        private readonly UserRepository _userRepository;


        public event EventHandler EmployeeAdded;

        public AddEmployeeViewModel()
        {
            AddUserCommand = new ViewModelCommand(AddUser, CanAddUser);
            _addUserRepository = new AddEmployee();
            _userRepository = new UserRepository();
        }

        private bool CanAddUser(object obj)
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                   !string.IsNullOrWhiteSpace(Surname) &&
                   !string.IsNullOrWhiteSpace(Contact) &&
                   !string.IsNullOrWhiteSpace(Address) &&
                   !string.IsNullOrWhiteSpace(Position) &&
                   WorkingHours > 0 && WorkingHours <= 50 &&
                   PayingHour > 0 &&
                   DateOfStart.HasValue &&
                   DateOfStart.Value >= new DateTime(2018, 1, 1) &&
                   DateOfStart.Value <= DateTime.Today;
        }

        public void AddUser(object obj)
        {
            try
            {
                var newUser = new Zaposleni
                {
                    ID = ID,
                    Name = Name,
                    Surname = Surname,
                    Contact = Contact,
                    Address = Address,
                    Position = Position,
                    WorkingHours = WorkingHours,
                    PayingHour = PayingHour,
                    DateOfStart = DateOfStart.Value,
                    Odeljenje = new Odeljenje { ID = CurrentUserAccount.Odeljenje.ID, Name = CurrentUserAccount.Odeljenje.Name }
                };

                _addUserRepository.Add(newUser);

                OnEmployeeAdded();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding employee: {ex.Message}");
            }
        }

        protected virtual void OnEmployeeAdded()
        {
            EmployeeAdded?.Invoke(this, EventArgs.Empty);
        }

        private void ClearFields()
        {
            ID = 0;
            Name = string.Empty;
            Surname = string.Empty;
            Contact = string.Empty;
            Address = string.Empty;
            Position = string.Empty;
            WorkingHours = 0;
            PayingHour = 0;
            DateOfStart = null;
            Odeljenje = null;
        }
    }
}



