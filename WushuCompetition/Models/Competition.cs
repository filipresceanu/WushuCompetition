using System.ComponentModel.DataAnnotations.Schema;

namespace WushuCompetition.Models
{
    public class Competition
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public ICollection<Category> Categories { get; set; }

       
    }
}
