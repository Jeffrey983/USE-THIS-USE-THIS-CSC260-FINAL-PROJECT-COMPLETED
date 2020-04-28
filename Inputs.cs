using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Windows.Forms;

namespace Snake
{
    internal class Input
    {
        private static Hashtable keyTable = new Hashtable(); //Loads a list of available Keyboard buttons for the program to use

        public static bool KeyPressed(Keys key) //Perform a check to see if a particular button is pressed.
        {
            if (keyTable[key] == null)
            {
                return false;
            }

            return (bool)keyTable[key];
        }

       
        public static void ChangeState(Keys key, bool state)  //Detects if a keyboard button is pressed
        {
            keyTable[key] = state;
        }
    }
}