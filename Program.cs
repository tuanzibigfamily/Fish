using GameLib;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MyFishGame
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            CMainWindow game = new CMainWindow();
            game.Strat(typeof(CFish), 20);
            
        }
    }
}
