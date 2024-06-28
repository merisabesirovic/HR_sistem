using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
//interfejs za implementiranje UserRepository klase
namespace HR_sistem.Models
{
    public interface IUserRepository
    {
        bool AuthenticateUser(NetworkCredential credential);
        RadnikHR GetbyUserName(string username);
        ObservableCollection<Zaposleni> GetEmployeesByOdeljenje(int departmentId);

    }
}
