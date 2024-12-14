using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopRite.Application.Services.Interfaces.Logging
{
    //this should be generic so that its reusable across system
    public interface IAppLogger<T>
    {
        void LogInformation(string message);  

        void LogError(Exception ex,string message);

        void LogWarning(string message);
    }
}
