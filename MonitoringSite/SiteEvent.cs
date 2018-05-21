using System;
using System.Collections.Generic;
using System.Text;

namespace MonitoringSite
{
    public class SiteEvent
    {
        /// <summary>
        /// Time of event
        /// </summary>
        public DateTime EventTime { get; set; }

        /// <summary>
        /// Message of the event
        /// </summary>
        public string EventMessage { get; set; }

        public SiteEvent()
        {

        }

        public SiteEvent(string message)
        {
            EventTime = DateTime.Now;
            EventMessage = message;
        }
    }
}
