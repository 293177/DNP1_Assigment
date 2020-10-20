using System.Collections.Generic;
using DNP1_A1.Models;

namespace DNP1_A1.Data
{
    public interface IAdultService
    {
        IList<Adult> GetAdults();
        void AddAdult(Adult adult);
        void RemoveAdult(Adult Radult);
        void EditAdult();
    }
}