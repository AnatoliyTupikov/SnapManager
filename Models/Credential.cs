using System;

namespace SnapManager.Models
{
    public class Credential
    {
        public int CredetialId { get; set; }
        public string Username { get; set; }

        public string Password { get; set; }

        public DateTime ModificationDate { get; set; }
    }
}
