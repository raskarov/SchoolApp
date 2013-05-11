using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolApp.ViewModels
{
    public class EventDetails
    {
        public int GroupInstanceId { get; set; }
        public string StartDateTime { get; set; }
        public string EndDateTime { get; set; }
        public List<String> Students { get; set; }
        public List<String> Teachers { get; set; }
    }
}