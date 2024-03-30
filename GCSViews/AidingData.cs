using MissionPlanner.Controls;
using MissionPlanner.Properties;
using MissionPlanner.Utilities;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace MissionPlanner.GCSViews
{
    public partial class AidingData : Form
    {
        public AidingData()
        {
            InitializeComponent();
        }

        private void AidingData_Load(object sender, EventArgs e)
        {
        }

        private void externalPositionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (externalPositionCheckBox.Checked == true)
            {
                externalPositionGroupBox.Enabled = true;
            }
            if (externalPositionCheckBox.Checked == false)
            {
                externalPositionGroupBox.Enabled = false;
            }
        }

        private void externalHorizontalPositionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (externalHorizontalPositionCheckBox.Checked == true)
            {
                externalHorizontalPositionGroupBox.Enabled = true;
            }
            if (externalHorizontalPositionCheckBox.Checked == false)
            {
                externalHorizontalPositionGroupBox.Enabled = false;
            }
        }

        private void altitudeExternalCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (altitudeExternalCheckBox.Checked == true)
            {
                altitudeExternalGroupBox.Enabled = true;
            }
            if (altitudeExternalCheckBox.Checked == false)
            {
                altitudeExternalGroupBox.Enabled = false;
            }
        }

        private void windDataCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (windDataCheckBox.Checked == true)
            {
                windDataGroupBox.Enabled = true;
            }
            if (windDataCheckBox.Checked == false)
            {
                windDataGroupBox.Enabled = false;
            }
        }

        private void ambientAirDataCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ambientAirDataCheckBox.Checked == true)
            {
                ambientAirDataGroupBox.Enabled = true;
            }
            if (ambientAirDataCheckBox.Checked == false)
            {
                ambientAirDataGroupBox.Enabled = false;
            }
        }

        private void headingExternalCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (headingExternalCheckBox.Checked == true)
            {
                headingExternalGroupBox.Enabled = true;
            }
            if (headingExternalCheckBox.Checked == false)
            {
                headingExternalGroupBox.Enabled = false;
            }
        }

        private void BUT_sendtoahrs_Click(object sender, EventArgs e)
        {
            /*stub*/
        }
    }
}