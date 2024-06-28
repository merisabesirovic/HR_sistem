using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR_sistem.Views;
using HR_sistem.Repositories;
using HR_sistem.Models;
using System.Threading;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using System.IO;
//view model za glavi prozor, ali ima svega i svaceg
//sve komande sa glavnog prozora su smestene ovde + reviews komande
namespace HR_sistem.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private RadnikHR _currentUserAccount;
        private IUserRepository _userRepository;
        private ObservableCollection<Zaposleni> _employees;
        private readonly ReportsRepo _reportsRepo;
        private ObservableCollection<PerformanceReview> _reviews;
        private Zaposleni _selectedEmployee;

        private int _newReviewScore;
        private string _newReviewComment;

        public ICommand ViewReviewsCommand { get; }
        public ICommand ShowWindowCommand { get; set; }
        public ICommand EditWindowCommand { get; set; }
        public ICommand AddReviewCommand { get; set; }
        public ICommand CalculateCommand { get; set; }
        public ICommand GenerateAvgScore { get; set; }
        public ICommand GenerateReport { get; set; }
        public ICommand GenerateChart { get; set; }
        public ICommand ViewBenefitsCommand { get; set; }

        public ObservableCollection<PerformanceReview> Reviews
        {
            get { return _reviews; }
            set { _reviews = value; OnPropertyChanged(nameof(Reviews)); }
        }

        public ObservableCollection<Zaposleni> Employees
        {
            get { return _employees; }
            set { _employees = value; OnPropertyChanged(nameof(Employees)); }
        }

        public RadnikHR CurrentUserAccount
        {
            get { return _currentUserAccount; }
            set { _currentUserAccount = value; OnPropertyChanged(nameof(CurrentUserAccount)); }
        }

        public Zaposleni SelectedEmployee
        {
            get { return _selectedEmployee; }
            set { _selectedEmployee = value; OnPropertyChanged(nameof(SelectedEmployee)); }
        }

        public int NewReviewScore
        {
            get { return _newReviewScore; }
            set { _newReviewScore = value; OnPropertyChanged(nameof(NewReviewScore)); }
        }

        public string NewReviewComment
        {
            get { return _newReviewComment; }
            set { _newReviewComment = value; OnPropertyChanged(nameof(NewReviewComment)); }
        }

        public MainViewModel()
        {
            _userRepository = new UserRepository();
            CurrentUserAccount = new RadnikHR();
            LoadCurrentUserData();
            LoadEmployees();
            ShowWindowCommand = new ViewModelCommand(ShowWindow, CanShowWindow);
            EditWindowCommand = new ViewModelCommand(EditWindow, CanShowWindow);
            _reportsRepo = new ReportsRepo();
            ViewReviewsCommand = new ViewModelCommand(ViewReviews);
            AddReviewCommand = new ViewModelCommand(AddReview);
            CalculateCommand = new ViewModelCommand(CalculatePayment);
            GenerateAvgScore = new ViewModelCommand(AverageScore);
            GenerateReport = new ViewModelCommand(GenerateReportForEmployee);
            ViewBenefitsCommand = new ViewModelCommand(ShowBenefitsWindow);
            GenerateChart = new ViewModelCommand(ShowChartWindow);
        }

        private void ShowChartWindow(object obj)
        {
            if (SelectedEmployee != null)
            {
                var reviews = _reportsRepo.GetReviews(SelectedEmployee.ID);
                if (reviews != null && reviews.Any())
                {
                    var chartViewModel = new ChartViewModel(reviews);

                    var chartWindow = new Chart
                    {
                        DataContext = chartViewModel
                    };
                    chartWindow.Show();
                }
                else
                {
                    MessageBox.Show("No reviews found for the selected employee.");
                }
            }
            else
            {
                MessageBox.Show("Please select an employee.");
            }
        }


        private void ShowBenefitsWindow(object obj)
        {
            if (obj is Zaposleni selectedEmployee)
            {
                var benefitsViewModel = new BenefitsViewModel
                {
                    SelectedEmployee = selectedEmployee
                };

                var benefitsWindow = new BenefitsWindow
                {
                    DataContext = benefitsViewModel
                };
                benefitsWindow.Show();
            }
            else
            {
                MessageBox.Show("Please select an employee.");
            }
        }
    



    private void CalculatePayment(object parameter)
        {
            if (parameter is Zaposleni employee)
            {
                double workingHours = employee.WorkingHours;
                double paymentPerHour = employee.PayingHour;
                double calculatedPayment = workingHours * paymentPerHour * 4;
                MessageBox.Show($"The calculated payment this month for {employee.Name} {employee.Surname} is: {calculatedPayment}");
            }
            else
            {
                MessageBox.Show("Please select a valid employee.");
            }
        }

        private void AddReview(object parameter)
        {
            if (SelectedEmployee != null)
            {
                var review = new PerformanceReview
                {
                    ReviewDate = DateTime.Now,
                    Score = NewReviewScore,
                    Comments = NewReviewComment
                };
                if (NewReviewScore > 0 && NewReviewScore <= 5 && NewReviewComment != null)
                {
                    _reportsRepo.AddReview(review, SelectedEmployee.ID);
                    Reviews.Add(review);
                    NewReviewScore = 0;
                    NewReviewComment = string.Empty;
                }
                else
                {
                    MessageBox.Show("Please fill out fields correctly");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Please select an employee.");
            }
        }
        private void AverageScore(object parameter)
        {
            if (SelectedEmployee != null)
            {
                Reviews = _reportsRepo.GetReviews(SelectedEmployee.ID);
                if (Reviews != null && Reviews.Any())
                {
                    double averageScore = Reviews.Average(review => review.Score);
                    MessageBox.Show($"The average score for {SelectedEmployee.Name} {SelectedEmployee.Surname} is: {averageScore:F2}");
                }
                else
                {
                    MessageBox.Show("No reviews found for the selected employee.");
                }
            }
            else
            {
                MessageBox.Show("Please select an employee.");
            }
        }
        private void GenerateReportForEmployee(object obj)
        {
            if (SelectedEmployee != null)
            {
                Reviews = _reportsRepo.GetReviews(SelectedEmployee.ID);
                if (Reviews != null && Reviews.Any())
                {
                    try
                    {
                        string relativePath = $"Reports/{SelectedEmployee.Name}_{SelectedEmployee.Surname}_Reviews.txt";
                        string directoryPath = Path.GetDirectoryName(relativePath);
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        using (TextWriter tw = new StreamWriter(relativePath))
                        {
                            tw.WriteLine($"Performance Reviews for {SelectedEmployee.Name} {SelectedEmployee.Surname}");
                            tw.WriteLine("-----------------------------------------------------");
                            foreach (var review in Reviews)
                            {
                                tw.WriteLine($"Date: {review.ReviewDate}");
                                tw.WriteLine($"Score: {review.Score}");
                                tw.WriteLine($"Comments: {review.Comments}");
                                tw.WriteLine("-----------------------------------------------------");
                            }
                        }

                        MessageBox.Show($"Report generated successfully at: {relativePath}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while generating the report: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("No reviews found for the selected employee.");
                }
            }
            else
            {
                MessageBox.Show("Please select an employee.");
            }
        }

        private void ViewReviews(object parameter)
        {
            if (parameter is Zaposleni employee)
            {
                SelectedEmployee = employee;
                Reviews = _reportsRepo.GetReviews(employee.ID);
                ShowReviewsWindow();
            }
        }

        private void ShowReviewsWindow()
        {
            var reviewsWindow = new ReviewsWindow
            {
                DataContext = this
            };
            reviewsWindow.ShowDialog();
        }

        private void EditWindow(object obj)
        {
            EditEmployeeView editEmployee = new EditEmployeeView();
            var employeeViewModel = new EditEmployeeViewModel();
            employeeViewModel.SetEmployees(Employees);
            editEmployee.DataContext = employeeViewModel;
            editEmployee.Show();
        }

        private bool CanShowWindow(object obj) => true;

        private void ShowWindow(object obj)
        {
            AddEmployeeView addEmployee = new AddEmployeeView();
            var addUserViewModel = (AddEmployeeViewModel)addEmployee.DataContext;
            addUserViewModel.CurrentUserAccount = CurrentUserAccount;
            addUserViewModel.EmployeeAdded += (s, e) => LoadEmployees();
            addEmployee.Show();
        }

        private void LoadCurrentUserData()
        {
            var user = _userRepository.GetbyUserName(Thread.CurrentPrincipal.Identity.Name);
            if (user != null)
            {
                CurrentUserAccount.ID = user.ID;
                CurrentUserAccount.Name = user.Name;
                CurrentUserAccount.Surname = user.Surname;
                CurrentUserAccount.UserName = user.UserName;
                CurrentUserAccount.Odeljenje = user.Odeljenje;
            }
            else
            {
                CurrentUserAccount.Name = "Invalid user, not logged in";
            }
        }

        public void LoadEmployees()
        {
            var allEmployees = _userRepository.GetEmployeesByOdeljenje(CurrentUserAccount.Odeljenje.ID);
            Employees = new ObservableCollection<Zaposleni>(allEmployees.Where(e => e.ID != CurrentUserAccount.ID));
        }
    }
}


