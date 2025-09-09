using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Ketarin.Forms
{
    public partial class AddCustomColumnDialog : Form
    {
        #region Properties

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ColumnName
        {
            get
            {
                return txtColumnName.Text.TrimStart('{').TrimEnd('}');
            }
            set
            {
                txtColumnName.Text = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ColumnValue
        {
            get
            {
                return txtColumnValue.Text;
            }
            set
            {
                txtColumnValue.Text = value;
            }
        }

        #endregion

        public AddCustomColumnDialog()
        {
            InitializeComponent();

            AcceptButton = bOK;
            CancelButton = bCancel;
        }

        private void txtColumnValue_TextChanged(object sender, EventArgs e)
        {
            bOK.Enabled = (!string.IsNullOrEmpty(txtColumnValue.Text));
        }
    }
}
