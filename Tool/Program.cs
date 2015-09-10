using System;
using System.Windows.Forms;


namespace Tool
{
    class Program{

        [STAThreadAttribute]
        static void Main(string [] args){

        //open form window
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new MainForm());
        
        }//end Main
    }//end class
}//end namespace
