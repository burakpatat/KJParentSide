using System.Collections;
using System.Collections.Generic;

namespace _Environments._Local 
{ 
    [System.Serializable]
    public class GuestProfileData
    {
        public string GUESTUUID;
        public GuestProfileData(string UUID)
        {
            GUESTUUID = UUID;
        }
    }
}
