using System;
using System.Drawing;
using System.Windows.Forms;
using InertialLabs.Embedders;
using MissionPlanner;

namespace InertialLabs.Forms
{
    public partial class EAHRSStatus : Form
    {
        private InertialLabs.Embedders.EahrsHudStatus eahrsHudStatusRef;
        private double eahrsType;
        private readonly string formTitle;

        public EAHRSStatus(InertialLabs.Embedders.EahrsHudStatus eahrsHudStatus, double eahrsTypeRef)
        {
            eahrsType = eahrsTypeRef;
            eahrsHudStatusRef = eahrsHudStatus;

            InitializeComponent();
            formTitle = Text;
            this.Height = 700;
            MissionPlanner.Utilities.ThemeManager.ApplyThemeTo(this);
            this.StartPosition = FormStartPosition.CenterScreen;

            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (eahrsType != 5 /*Inertial Labs EAHRS*/)
            {
                return;
            }

            bool hasData = PluginState.IsEahrsStatusInfoFresh;
            string title = hasData ? formTitle : formTitle + " - NO DATA";
            if (Text != title)
            {
                Text = title;
            }

            var rows = EahrsStatusTable.GetStatusRows();
            for (int idx = 0; idx < rows.Count; idx++)
            {
                if (flowLayoutPanel1.Controls.Count <= idx)
                {
                    flowLayoutPanel1.Controls.Add(new Label() { Height = 13, Width = flowLayoutPanel1.Width });
                }

                var row = rows[idx];
                flowLayoutPanel1.Controls[idx].Text = row.Name + " " + row.Value + "\r\n";
                flowLayoutPanel1.Controls[idx].ForeColor = hasData ? RowColor(row.Severity) : EahrsHudStatus.NoDataColor;
            }
        }

        private Color RowColor(EahrsSeverity severity)
        {
            switch (severity)
            {
                case EahrsSeverity.Good:
                    return Color.Green;
                case EahrsSeverity.Info:
                    return Color.DarkCyan;
                case EahrsSeverity.Warning:
                    return Color.Orange;
                case EahrsSeverity.Fail:
                    return Color.Red;
                default:
                    return ForeColor;
            }
        }
    }
}