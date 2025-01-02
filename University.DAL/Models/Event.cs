using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.DAL.Models
{
    public class Event
    {
        public int eventID {  get; set; }
        public string eventName { get; set; }
        public DateTime date { get; set; }
        public string time {  get; set; }

    }
}
