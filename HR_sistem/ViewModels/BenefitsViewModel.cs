using HR_sistem.Models;
using HR_sistem.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace HR_sistem.ViewModels
{
    public class BenefitsViewModel : ViewModelBase
    {
        private Zaposleni _selectedEmployee;
        private BenefitsRepo _benefitsRepo;
        private ReportsRepo _reportsRepo;
        private ObservableCollection<PerformanceReview> _performanceReviews;

        private string _newBenefitName;
        private DateTime? _newBenefitGrantedDate;
        private DateTime? _newBenefitEndDate;

        public Zaposleni SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged(nameof(SelectedEmployee));
                LoadBenefits();
            }
        }

        public ObservableCollection<Benefits> Benefits { get; set; }

        public string NewBenefitName
        {
            get { return _newBenefitName; }
            set { _newBenefitName = value; OnPropertyChanged(nameof(NewBenefitName)); }
        }

        public DateTime? NewBenefitGrantedDate
        {
            get { return _newBenefitGrantedDate; }
            set { _newBenefitGrantedDate = value; OnPropertyChanged(nameof(NewBenefitGrantedDate)); }
        }

        public DateTime? NewBenefitEndDate
        {
            get { return _newBenefitEndDate; }
            set { _newBenefitEndDate = value; OnPropertyChanged(nameof(NewBenefitEndDate)); }
        }

        public ICommand AddBenefitCommand { get; set; }

        public BenefitsViewModel()
        {
            _benefitsRepo = new BenefitsRepo();
            _reportsRepo = new ReportsRepo();
            Benefits = new ObservableCollection<Benefits>();
            AddBenefitCommand = new ViewModelCommand(AddBenefit);
        }

        private void LoadBenefits()
        {
            if (SelectedEmployee != null)
            {
                Benefits.Clear();
                var benefits = _benefitsRepo.GetBenefits(SelectedEmployee.ID);
                foreach (var benefit in benefits)
                {
                    Benefits.Add(benefit);
                }
            }
        }

        public double GetAverageScore()
        {
            if (SelectedEmployee != null)
            {
                _performanceReviews = _reportsRepo.GetReviews(SelectedEmployee.ID);
                if (_performanceReviews != null && _performanceReviews.Any())
                {
                    double averageScore = _performanceReviews.Average(item => item.Score);
                    return averageScore;
                }
            }
            return 0;
        }

        private void AddBenefit(object parameter)
        {
            if (SelectedEmployee != null &&
                !string.IsNullOrEmpty(NewBenefitName) &&
                NewBenefitGrantedDate.HasValue &&
                NewBenefitEndDate.HasValue && 
                NewBenefitEndDate.Value > NewBenefitGrantedDate &&
                GetAverageScore() >= 3)
            {
                var newBenefit = new Benefits
                {
                    BenefitName = NewBenefitName,
                    GrantedDate = NewBenefitGrantedDate.Value,
                    EndedDate = NewBenefitEndDate.Value,
                };

                _benefitsRepo.AddBenefit(newBenefit, SelectedEmployee.ID);
                Benefits.Add(newBenefit);

                NewBenefitName = string.Empty;
                NewBenefitGrantedDate = null;
                NewBenefitEndDate = null;
            }
            else
            {
                MessageBox.Show("Please fill in all fields correctly and check if this employee has average score of 3 of more");
            }
        }
    }
}

