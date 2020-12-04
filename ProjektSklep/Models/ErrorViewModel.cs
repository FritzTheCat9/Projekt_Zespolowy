using System;

namespace ProjektSklep.Models
{
    public class ErrorViewModel
    {
        /* POLA */
        public string RequestId { get; set; }

        /* METODY */
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
