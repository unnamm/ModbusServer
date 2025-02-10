using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.View;

namespace UI
{
    internal class Starter
    {
        [STAThread]
        private static void Main()
        {
            new App().Run(); //run App.Startup event
        }
    }
}
