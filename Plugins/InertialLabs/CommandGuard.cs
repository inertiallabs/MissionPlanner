using System;
using System.Windows.Forms;

namespace InertialLabs
{
    internal static class CommandGuard
    {
        private static bool busy;

        public static void Run(Action command)
        {
            if (busy)
            {
                return;
            }

            busy = true;
            try
            {
                command();
            }
            finally
            {
                Application.Idle -= OnIdle;
                Application.Idle += OnIdle;
            }
        }

        private static void OnIdle(object sender, EventArgs e)
        {
            Application.Idle -= OnIdle;
            busy = false;
        }
    }
}
