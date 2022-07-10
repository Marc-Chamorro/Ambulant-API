using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace API.Common_Functions
{
    public class TextValidation
    {
        public bool CheckAsciiCharacters(string content, bool contains_space = true)
        {
            int first_character = 32;
            if (!contains_space)
            {
                first_character = 33;
            }

            for (int i = 0; i < content.Length; i++)
            {
                if (content[i] >= first_character && content[i] <= 126  == false)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
