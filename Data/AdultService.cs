using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using DNP1_A1.Models;

namespace DNP1_A1.Data
{
    public class AdultService : IAdultService
    {
        private string adultFile = "adults.json";
        public IList<Adult> adults { get; set; }

        public AdultService()
        {
            string content = File.ReadAllText(adultFile);
            adults = JsonSerializer.Deserialize<IList<Adult>>(content);
        }

        public IList<Adult> GetAdults()
        {
            return adults;
        }

        public void AddAdult(Adult adult)
        {
            adults.Add(adult);
            WriteToFile();
        }

        private void WriteToFile()
        {
            string productAsJson = JsonSerializer.Serialize(adults);
            File.WriteAllText(adultFile, productAsJson);
        }

        public void RemoveAdult(Adult Radult)
        {
            Adult toRemove1 = adults.First(t => t.Id == Radult.Id);
            adults.Remove(Radult);
            WriteToFile();
        }

        public void EditAdult()
        {
            throw new System.NotImplementedException();
        }
    }
}