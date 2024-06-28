using HR_sistem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using HR_sistem.Repositories;
using System.Collections.ObjectModel;
//view model za editovanje selektovanog usera ili brisanje
namespace HR_sistem.ViewModels
{
    public class EditEmployeeViewModel : ViewModelBase
    {
        private Zaposleni _selectedEmployee;
        private EditEmployee _editEmployeeRepository;

        public Zaposleni SelectedEmployee
        {
            get { return _selectedEmployee; }
            set { _selectedEmployee = value; OnPropertyChanged(nameof(SelectedEmployee)); }
        }

        public ObservableCollection<Zaposleni> Employees { get; set; }

        public ICommand EditEmployeeCommand { get; set; }
        public ICommand DeleteEmployeeCommand { get; set; }

        public EditEmployeeViewModel()
        {
            _editEmployeeRepository = new EditEmployee();
            EditEmployeeCommand = new ViewModelCommand(EditEmployee, CanEditEmployee);
            DeleteEmployeeCommand = new ViewModelCommand(DeleteEmployee, CanDeleteEmployee);
            Employees = new ObservableCollection<Zaposleni>(); 
        }

        private bool CanEditEmployee(object obj)
        {
            return SelectedEmployee != null;
        }

        private void EditEmployee(object obj)
        {
            if (SelectedEmployee != null)
            {
                _editEmployeeRepository.Edit(SelectedEmployee);
                MessageBox.Show("Employee has been succesfully edited");

            }
        }

        private bool CanDeleteEmployee(object obj)
        {
            return SelectedEmployee != null;
        }

        private void DeleteEmployee(object obj)
        {
            if (SelectedEmployee != null && ShowConfirmationDialog())
            {
                _editEmployeeRepository.Delete(SelectedEmployee.ID);
                Employees.Remove(SelectedEmployee);
                SelectedEmployee = null;
            }
        }

        private bool ShowConfirmationDialog()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this employee?",
                                                      "Confirm Delete",
                                                      MessageBoxButton.YesNo,
                                                      MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }

        public void SetEmployees(ObservableCollection<Zaposleni> employees)
        {
            Employees = employees;
            OnPropertyChanged(nameof(Employees));
        }
    }
}


